<template>
  <q-layout view="hHr LpR lFf">
    <q-page-container>
      <q-page class="flex flex-center">
        <q-card class="login-card shadow-2">
          <q-card-section class="bg-primary text-white">
            <div class="text-h6 text-center">Login</div>
          </q-card-section>

          <q-card-section>
            <q-form @submit="handleLogin" class="q-gutter-md">
              <q-input
                v-model="loginForm.usuario"
                label="Username"
                :disable="loading"
                :rules="[(val) => !!val || 'Username is required']"
                filled>
                <template v-slot:prepend>
                  <q-icon name="person" />
                </template>
              </q-input>

              <q-input
                v-model="loginForm.contrasena"
                label="Password"
                :disable="loading"
                :rules="[(val) => !!val || 'Password is required']"
                filled
                :type="isPwd ? 'password' : 'text'">
                <template v-slot:prepend>
                  <q-icon name="lock" />
                </template>
                <template v-slot:append>
                  <q-icon
                    :name="isPwd ? 'visibility_off' : 'visibility'"
                    class="cursor-pointer"
                    @click="isPwd = !isPwd" />
                </template>
              </q-input>

              <div class="text-negative" v-if="error">{{ error }}</div>

              <div class="flex justify-center q-mt-md">
                <q-btn
                  type="submit"
                  color="primary"
                  label="Login"
                  :loading="loading"
                  :disable="!isFormValid"
                  class="full-width" />
              </div>
            </q-form>
          </q-card-section>
        </q-card>
      </q-page>
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
  import { ref, computed } from 'vue'
  import { useRouter } from 'vue-router'
  import { LoginService, LoginRequest } from 'src/services/LoginService'
  import { useAuthStore } from 'src/stores/authStore'
  import { useQuasar } from 'quasar'
  import { type AxiosError } from 'axios'

  const router = useRouter()
  const authStore = useAuthStore()
  const $q = useQuasar()

  const loginForm = ref<LoginRequest>({
    usuario: '',
    contrasena: '',
  })

  const loading = ref(false)
  const error = ref('')
  const isPwd = ref(true)

  const isFormValid = computed(() => {
    return loginForm.value.usuario.trim() !== '' && loginForm.value.contrasena.trim() !== ''
  })

  const handleLogin = async () => {
    if (!isFormValid.value) return

    error.value = ''
    loading.value = true

    try {
      const response = await LoginService.Login(loginForm.value)
      console.log('response: ', response)
      authStore.token = response.token
      console.log('authStore.token : ', authStore.token)

      $q.notify({
        type: 'positive',
        message: 'Login successful!',
      })

      // Redirect to home or dashboard after successful login
      router.push('/dashboard')
    } catch (err) {
      error.value = (err as AxiosError).response?.data + '' || 'Failed to login. Please try again.'

      $q.notify({
        type: 'negative',
        message: error.value,
      })
    } finally {
      loading.value = false
    }
  }
</script>

<style scoped>
  .login-card {
    width: 100%;
    max-width: 400px;
  }
</style>
