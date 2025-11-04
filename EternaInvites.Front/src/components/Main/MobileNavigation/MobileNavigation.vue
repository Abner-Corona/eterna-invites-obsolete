<template>
  <q-drawer v-model="mainStore.mobileMenuOpen" side="right" overlay behavior="mobile" class="lt-md mobile-nav-drawer">
    <q-list>
      <q-item-label header>{{ $t('header.navegacion') }}</q-item-label>

      <q-item clickable @click="handleNavigation('/')">
        <q-item-section avatar>
          <q-icon name="home" />
        </q-item-section>
        <q-item-section>
          <q-item-label>{{ $t('header.inicio') }}</q-item-label>
        </q-item-section>
      </q-item>

      <q-item clickable @click="handleNavigation('/templates')">
        <q-item-section avatar>
          <q-icon name="template" />
        </q-item-section>
        <q-item-section>
          <q-item-label>{{ $t('header.plantillas') }}</q-item-label>
        </q-item-section>
      </q-item>

      <q-item clickable @click="handleNavigation('/demos')">
        <q-item-section avatar>
          <q-icon name="preview" />
        </q-item-section>
        <q-item-section>
          <q-item-label>{{ $t('header.demos') }}</q-item-label>
        </q-item-section>
      </q-item>

      <q-item clickable @click="handleNavigation('/contact')">
        <q-item-section avatar>
          <q-icon name="contact_mail" />
        </q-item-section>
        <q-item-section>
          <q-item-label>{{ $t('header.contacto') }}</q-item-label>
        </q-item-section>
      </q-item>

      <q-separator class="q-my-md" />

      <q-item-label header>{{ $t('header.idioma') }}</q-item-label>

      <LanguageSelector class="full-width" />
    </q-list>
  </q-drawer>
</template>

<script setup lang="ts">
  import { defineAsyncComponent } from 'vue'
  import { useRouter } from 'vue-router'
  import { useMainStore } from 'src/stores/mainStore'

  // Async Components
  const LanguageSelector = defineAsyncComponent(() => import('src/components/Main/LanguageSelector/LanguageSelector.vue'))

  // Composables
  const router = useRouter()
  const mainStore = useMainStore()

  // Methods
  const handleNavigation = (path: string) => {
    router.push(path)
    mainStore.handleNavigation(path)
  }
</script>

<style scoped lang="scss" src="./MobileNavigation.scss"></style>
