<template>
  <q-layout view="hHr LpR lFf">
    <div class="landing-page">
      <!-- Component Sections -->
      <HeroSection />
      <FeaturesSection />
      <DemosSection />
      <CtaSection />
      <NavigationMenu />
    </div>
  </q-layout>
</template>

<script setup lang="ts">
  import { onMounted, defineAsyncComponent } from 'vue'
  import { useI18n } from 'vue-i18n'

  // Using defineAsyncComponent for lazy loading
  const HeroSection = defineAsyncComponent(() => import('src/components/Main/HeroSection.vue'))
  const FeaturesSection = defineAsyncComponent(
    () => import('src/components/Main/FeaturesSection.vue'),
  )
  const DemosSection = defineAsyncComponent(() => import('src/components/Main/DemosSection.vue'))
  const CtaSection = defineAsyncComponent(() => import('src/components/Main/CtaSection.vue'))
  const NavigationMenu = defineAsyncComponent(
    () => import('src/components/Main/NavigationMenu.vue'),
  )

  // i18n setup
  const { locale } = useI18n()

  // Animation logic
  onMounted(() => {
    // Load saved language preference if available
    const savedLanguage = localStorage.getItem('userLanguage')
    if (savedLanguage) {
      locale.value = savedLanguage
    }

    // Observe elements with animation classes and trigger them when in viewport
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            entry.target.classList.add('animated')
            observer.unobserve(entry.target)
          }
        })
      },
      { threshold: 0.1 },
    )

    // Observe all elements with animation classes
    document
      .querySelectorAll('.animate-fade-in, .animate-from-bottom, .animate-scale')
      .forEach((el) => {
        observer.observe(el)
      })
  })
</script>

<style scoped>
  .landing-page {
    overflow-x: hidden;
  }

  /* Animation Classes */
  .animate-fade-in,
  .animate-from-bottom,
  .animate-scale {
    opacity: 0;
    transition: all 0.8s ease-out;
  }

  .animate-from-bottom {
    transform: translateY(50px);
  }

  .animate-scale {
    transform: scale(0.8);
  }

  .animate-bounce {
    animation: bounce 2s infinite;
  }

  .animated.animate-fade-in {
    opacity: 1;
  }

  .animated.animate-from-bottom {
    opacity: 1;
    transform: translateY(0);
  }

  .animated.animate-scale {
    opacity: 1;
    transform: scale(1);
  }

  @keyframes bounce {
    0%,
    20%,
    50%,
    80%,
    100% {
      transform: translateY(0);
    }
    40% {
      transform: translateY(-20px);
    }
    60% {
      transform: translateY(-10px);
    }
  }

  /* Pulse animation */
  .pulse-animation {
    animation: pulse 2s infinite;
  }

  @keyframes pulse {
    0% {
      box-shadow: 0 0 0 0 rgba(0, 0, 0, 0.2);
    }
    70% {
      box-shadow: 0 0 0 10px rgba(0, 0, 0, 0);
    }
    100% {
      box-shadow: 0 0 0 0 rgba(0, 0, 0, 0);
    }
  }
</style>
