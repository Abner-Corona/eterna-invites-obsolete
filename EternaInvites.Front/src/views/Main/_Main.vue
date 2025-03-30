<template>
  <q-layout view="hHr LpR lFf">
    <div class="landing-page">
      <!-- Hero Section -->
      <section class="hero-section">
        <div class="hero-content animate-fade-in">
          <h1 class="text-h2 text-white q-mb-md">{{ $t('main.heroTitle') }}</h1>
          <h2 class="text-h5 text-white q-mb-xl">{{ $t('main.heroSubtitle') }}</h2>
          <q-btn
            color="primary"
            size="lg"
            :label="$t('main.discoverButton')"
            class="animate-bounce" />
        </div>
      </section>

      <!-- Services Section -->
      <section class="services-section q-py-xl">
        <div class="q-container">
          <h2 class="text-h3 text-center q-mb-xl animate-from-bottom">
            {{ $t('main.servicesTitle') }}
          </h2>
          <div class="row q-col-gutter-md">
            <div v-for="(service, index) in services" :key="index" class="col-12 col-md-4">
              <q-card
                class="service-card animate-from-bottom"
                :style="`transition-delay: ${index * 0.1}s`">
                <q-img :src="service.img" height="200px" />
                <q-card-section>
                  <h3 class="text-h5 q-mb-sm">{{ $t(service.titleKey) }}</h3>
                  <p>{{ $t(service.descriptionKey) }}</p>
                </q-card-section>
              </q-card>
            </div>
          </div>
        </div>
      </section>

      <!-- Why Choose Us Section -->
      <section class="features-section q-py-xl bg-grey-2">
        <div class="q-container">
          <h2 class="text-h3 text-center q-mb-xl animate-from-bottom">
            {{ $t('main.featuresTitle') }}
          </h2>
          <div class="row q-col-gutter-lg">
            <div
              v-for="(feature, index) in features"
              :key="index"
              class="col-12 col-md-4 text-center">
              <div class="feature-item animate-scale" :style="`transition-delay: ${index * 0.1}s`">
                <q-icon :name="feature.icon" size="64px" color="primary" class="q-mb-md" />
                <h3 class="text-h5 q-mb-sm">{{ $t(feature.titleKey) }}</h3>
                <p>{{ $t(feature.descriptionKey) }}</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Testimonials Section -->
      <section class="testimonials-section q-py-xl">
        <div class="q-container">
          <h2 class="text-h3 text-center q-mb-xl animate-from-bottom">
            {{ $t('main.testimonialsTitle') }}
          </h2>
          <q-carousel
            v-model="testimonialSlide"
            animated
            arrows
            navigation
            infinite
            height="300px"
            class="animate-fade-in">
            <q-carousel-slide
              v-for="(testimonial, index) in testimonials"
              :key="index"
              :name="index">
              <div class="testimonial-content text-center q-pa-lg">
                <q-avatar size="100px" class="q-mb-md">
                  <img :src="testimonial.avatar" />
                </q-avatar>
                <p class="text-h6 q-mb-md">{{ $t(testimonial.textKey) }}</p>
                <h5 class="text-weight-bold">{{ $t(testimonial.nameKey) }}</h5>
                <p>{{ $t(testimonial.roleKey) }}</p>
              </div>
            </q-carousel-slide>
          </q-carousel>
        </div>
      </section>

      <!-- CTA Section -->
      <section class="cta-section q-py-xl">
        <div class="q-container text-center">
          <h2 class="text-h3 text-white q-mb-lg animate-from-bottom">
            {{ $t('main.ctaTitle') }}
          </h2>
          <p class="text-h6 text-white q-mb-xl animate-from-bottom" style="transition-delay: 0.1s">
            {{ $t('main.ctaSubtitle') }}
          </p>
          <q-btn
            color="white"
            text-color="primary"
            size="lg"
            :label="$t('main.contactButton')"
            class="animate-scale" />
        </div>
      </section>

      <!-- Floating Menu Button -->
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
    </div>
  </q-layout>
</template>

<script setup lang="ts">
  import { ref, onMounted, computed } from 'vue'
  import { useI18n } from 'vue-i18n'
  import { useRouter } from 'vue-router'
  import { useAuthStore } from 'src/stores/authStore' // Assuming auth store exists

  // Router setup
  const router = useRouter()

  // Auth state
  const authStore = useAuthStore()
  const isAuthenticated = computed(() => !!authStore.token)

  // Menu state
  const mainMenu = ref(false)

  // i18n setup
  const { locale } = useI18n()

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

  // Testimonial carousel
  const testimonialSlide = ref(0)

  // Services data
  const services = ref([
    {
      titleKey: 'main.services_digital_title',
      descriptionKey: 'main.services_digital_description',
      img: 'https://images.unsplash.com/photo-1511184117514-a1e9f0111e65?ixlib=rb-1.2.1&auto=format&fit=crop&w=500&q=60',
    },
    {
      titleKey: 'main.services_printed_title',
      descriptionKey: 'main.services_printed_description',
      img: 'https://images.unsplash.com/photo-1607344645866-d78bbbc1c919?ixlib=rb-1.2.1&auto=format&fit=crop&w=500&q=60',
    },
    {
      titleKey: 'main.services_stationery_title',
      descriptionKey: 'main.services_stationery_description',
      img: 'https://images.unsplash.com/photo-1547267346-671120edb6d0?ixlib=rb-1.2.1&auto=format&fit=crop&w=500&q=60',
    },
  ])

  // Features data
  const features = ref([
    {
      icon: 'diamond',
      titleKey: 'main.features_design_title',
      descriptionKey: 'main.features_design_description',
    },
    {
      icon: 'favorite',
      titleKey: 'main.features_attention_title',
      descriptionKey: 'main.features_attention_description',
    },
    {
      icon: 'schedule',
      titleKey: 'main.features_delivery_title',
      descriptionKey: 'main.features_delivery_description',
    },
  ])

  // Testimonials data
  const testimonials = ref([
    {
      nameKey: 'main.testimonials_couple1_name',
      roleKey: 'main.testimonials_couple1_role',
      textKey: 'main.testimonials_couple1_text',
      avatar:
        'https://images.unsplash.com/photo-1543610892-0b1f7e6d8ac1?ixlib=rb-1.2.1&auto=format&fit=crop&w=100&q=80',
    },
    {
      nameKey: 'main.testimonials_person1_name',
      roleKey: 'main.testimonials_person1_role',
      textKey: 'main.testimonials_person1_text',
      avatar:
        'https://images.unsplash.com/photo-1554151228-14d9def656e4?ixlib=rb-1.2.1&auto=format&fit=crop&w=100&q=80',
    },
    {
      nameKey: 'main.testimonials_couple2_name',
      roleKey: 'main.testimonials_couple2_role',
      textKey: 'main.testimonials_couple2_text',
      avatar:
        'https://images.unsplash.com/photo-1499952127939-9bbf5af6c51c?ixlib=rb-1.2.1&auto=format&fit=crop&w=100&q=80',
    },
  ])

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
  function addAbortListener() {}
</script>

<style scoped>
  .landing-page {
    overflow-x: hidden;
  }

  /* Hero Section */
  .hero-section {
    height: 100vh;
    background-image:
      linear-gradient(rgba(0, 0, 0, 0.4), rgba(0, 0, 0, 0.4)),
      url('https://images.unsplash.com/photo-1519741497674-611481863552?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80');
    background-size: cover;
    background-position: center;
    display: flex;
    align-items: center;
    justify-content: center;
    text-align: center;
    color: white;
  }

  .hero-content {
    max-width: 800px;
    padding: 0 20px;
  }

  /* Services Section */
  .service-card {
    height: 100%;
    transition: transform 0.3s;
  }

  .service-card:hover {
    transform: translateY(-10px);
  }

  /* CTA Section */
  .cta-section {
    background-image:
      linear-gradient(rgba(0, 0, 0, 0.6), rgba(0, 0, 0, 0.6)),
      url('https://images.unsplash.com/photo-1469371670807-013ccf25f16a?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80');
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
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

  /* Language button animation */
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
