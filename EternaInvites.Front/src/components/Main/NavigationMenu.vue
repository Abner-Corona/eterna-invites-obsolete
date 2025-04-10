<template>
  <q-page-sticky position="top-right" :offset="[20, 20]">
    <q-btn flat round text-color="black" icon="menu" class="bg-transparent language-btn">
      <q-menu v-model="mainMenu" anchor="top right" self="bottom right">
        <q-list style="min-width: 200px">
          <!-- Login/Dashboard Option -->
          <q-item clickable v-close-popup @click="handleAuthNavigation">
            <q-item-section avatar>
              <q-icon :name="isAuthenticated ? 'dashboard' : 'login'" />
            </q-item-section>
            <q-item-section>
              {{ isAuthenticated ? $t('main.goToDashboard') : $t('main.goToLogin') }}
            </q-item-section>
          </q-item>

          <!-- Language Selector -->
          <q-item clickable v-ripple>
            <q-item-section avatar>
              <q-icon name="language" />
            </q-item-section>
            <q-item-section>{{ $t('main.languageSelector') }}</q-item-section>
            <q-item-section side>
              <q-icon name="keyboard_arrow_right" />
            </q-item-section>

            <!-- Language Submenu -->
            <q-menu anchor="top end" self="top start">
              <q-list style="min-width: 150px">
                <q-item clickable v-close-popup @click="changeLanguage('es')">
                  <q-item-section avatar>
                    <q-icon name="flag" color="red" />
                  </q-item-section>
                  <q-item-section>{{ $t('main.esp') }}</q-item-section>
                </q-item>
                <q-item clickable v-close-popup @click="changeLanguage('en')">
                  <q-item-section avatar>
                    <q-icon name="flag" color="blue" />
                  </q-item-section>
                  <q-item-section>{{ $t('main.eng') }}</q-item-section>
                </q-item>
              </q-list>
            </q-menu>
          </q-item>
        </q-list>
      </q-menu>
    </q-btn>
  </q-page-sticky>
</template>

<script setup lang="ts">
  import { ref, computed } from 'vue'
  import { useRouter } from 'vue-router'
  import { useI18n } from 'vue-i18n'
  import { useAuthStore } from 'src/stores/authStore' // Assuming auth store exists

  // Router setup
  const router = useRouter()

  // i18n setup
  const { locale } = useI18n()

  // Auth state
  const authStore = useAuthStore()
  const isAuthenticated = computed(() => !!authStore.token)

  // Menu state
  const mainMenu = ref(false)

  // Handle navigation to login or dashboard
  const handleAuthNavigation = () => {
    if (isAuthenticated.value) {
      router.push('/dashboard')
    } else {
      router.push('/login')
    }
  }

  // Function to change language
  const changeLanguage = (lang: string) => {
    locale.value = lang
    // Optional: store language preference
    localStorage.setItem('userLanguage', lang)
  }
</script>

<style scoped>
  /* Language Button */
  .language-btn {
    backdrop-filter: blur(4px);
    transition: all 0.3s ease;
  }

  .language-btn:hover {
    background: rgba(255, 255, 255, 0.2) !important;
    transform: scale(1.1);
  }
</style>
