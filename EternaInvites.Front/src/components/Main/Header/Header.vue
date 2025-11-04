<template>
  <q-header :class="['header-transition text-dark', { 'header-scrolled': mainStore.getIsScrolled }]">
    <q-toolbar :class="{ 'toolbar-scrolled': mainStore.getIsScrolled }">
      <q-toolbar-title class="text-primary title-transition" :class="{ 'title-scrolled': mainStore.getIsScrolled }">
        <div class="row items-center no-wrap">
          <q-icon name="favorite" class="q-mr-sm text-pink icon-transition" :class="{ 'icon-scrolled': mainStore.getIsScrolled }" />
          <span class="text-weight-bold">Eterna Invites</span>
        </div>
      </q-toolbar-title>

      <q-space />

      <!-- Navigation Menu -->
      <div class="gt-sm">
        <q-btn flat no-caps :label="$t('header.inicio')" class="q-mx-sm nav-btn" :class="{ 'nav-btn-scrolled': mainStore.getIsScrolled }" />
        <q-btn flat no-caps :label="$t('header.plantillas')" class="q-mx-sm nav-btn" :class="{ 'nav-btn-scrolled': mainStore.getIsScrolled }" />
        <q-btn flat no-caps :label="$t('header.demos')" class="q-mx-sm nav-btn" :class="{ 'nav-btn-scrolled': mainStore.getIsScrolled }" />
        <q-btn flat no-caps :label="$t('header.contacto')" class="q-mx-sm nav-btn" :class="{ 'nav-btn-scrolled': mainStore.getIsScrolled }" />
      </div>

      <!-- Language Selector -->
      <LanguageSelector class="gt-sm" />

      <!-- Mobile Menu -->
      <q-btn flat dense round icon="menu" aria-label="Menu" class="lt-md" @click="toggleMobileMenu" />
    </q-toolbar>
  </q-header>
</template>

<script setup lang="ts">
  import { onMounted, onUnmounted, defineAsyncComponent } from 'vue'

  import { useMainStore } from 'src/stores/mainStore'

  // Async Components
  const LanguageSelector = defineAsyncComponent(() => import('src/components/Main/LanguageSelector/LanguageSelector.vue'))

  // Composables
  const mainStore = useMainStore()

  // Scroll handler
  const handleScroll = (evt?: Event) => {
    let scrollTop = 0

    // Si viene de q-scroll-area
    if (evt && evt.target) {
      const target = evt.target as HTMLElement
      scrollTop = target.scrollTop
    } else {
      // Fallback para scroll normal
      scrollTop = window.pageYOffset || document.documentElement.scrollTop
    }

    // Usar el store para manejar el scroll
    mainStore.handleScroll(scrollTop)
  }

  // Lifecycle hooks
  onMounted(() => {
    // Escuchar scroll del window (fallback)
    window.addEventListener('scroll', handleScroll, { passive: true })

    // Buscar y escuchar el q-scroll-area
    const findScrollArea = () => {
      const scrollArea = document.querySelector('.q-scrollarea__container')
      if (scrollArea) {
        scrollArea.addEventListener('scroll', handleScroll, { passive: true })
        return true
      }
      return false
    }

    // Intentar encontrar el scroll area inmediatamente
    if (!findScrollArea()) {
      // Si no se encuentra, intentar después de un breve delay
      setTimeout(findScrollArea, 100)
    }
  })

  onUnmounted(() => {
    window.removeEventListener('scroll', handleScroll)

    // Remover el listener del scroll area también
    const scrollArea = document.querySelector('.q-scrollarea__container')
    if (scrollArea) {
      scrollArea.removeEventListener('scroll', handleScroll)
    }
  })

  // Methods
  const toggleMobileMenu = () => {
    mainStore.toggleMobileMenu()
  }
</script>

<style scoped lang="scss" src="./Header.scss"></style>
