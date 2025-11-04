<template>
  <div class="container-2">
    <div class="row full-width q-pa-lg">
      <!-- Content Section -->
      <div class="col-12 flex column justify-center items-center q-pa-md">
        <div class="content-section" v-motion :initial="{ opacity: 0, y: 30 }" :enter="{ opacity: 1, y: 0 }" :duration="800" :delay="200">
          <h2 class="text-h3 text-primary q-mb-md section-title">
            {{ $t('container2.title') }}
          </h2>
          <p class="text-h6 text-grey-8 q-mb-lg section-subtitle">
            {{ $t('container2.subtitle') }}
          </p>

          <!-- Feature Grid -->
          <div class="feature-grid q-mt-xl">
            <div v-for="(feature, index) in features" :key="index" class="feature-card" v-motion :initial="{ opacity: 0, y: 20 }" :enter="{ opacity: 1, y: 0 }" :duration="600" :delay="400 + index * 100">
              <q-card class="feature-card-inner">
                <q-card-section class="text-center">
                  <q-icon :name="feature.icon" size="3rem" :color="feature.color" class="q-mb-md" />
                  <h3 class="text-h6 text-weight-bold q-mb-sm">
                    {{ feature.title }}
                  </h3>
                  <p class="text-body2 text-grey-7">
                    {{ feature.description }}
                  </p>
                </q-card-section>
              </q-card>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import { useI18n } from 'vue-i18n'

  // Composables
  const { t } = useI18n()

  // Reactive data
  const features = ref([
    {
      icon: 'design_services',
      color: 'primary',
      title: t('container2.features.design.title'),
      description: t('container2.features.design.description'),
    },
    {
      icon: 'palette',
      color: 'secondary',
      title: t('container2.features.customization.title'),
      description: t('container2.features.customization.description'),
    },
    {
      icon: 'share',
      color: 'accent',
      title: t('container2.features.sharing.title'),
      description: t('container2.features.sharing.description'),
    },
    {
      icon: 'mobile_friendly',
      color: 'positive',
      title: t('container2.features.responsive.title'),
      description: t('container2.features.responsive.description'),
    },
  ])
</script>

<style scoped lang="scss" src="./Container2.scss"></style>
