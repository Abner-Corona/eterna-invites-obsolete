<template>
  <section class="features-section q-py-xl bg-grey-2">
    <div class="q-container">
      <h2 class="text-h3 text-center q-mb-xl animate-from-bottom">
        {{ $t('main.featuresTitle') }}
      </h2>
      <div class="row q-col-gutter-lg">
        <div v-for="(feature, index) in features" :key="index" class="col-12 col-md-4 text-center">
          <div class="feature-item animate-scale" :style="`transition-delay: ${index * 0.1}s`">
            <q-icon :name="feature.icon" size="64px" color="primary" class="q-mb-md" />
            <h3 class="text-h5 q-mb-sm">{{ $t(feature.titleKey) }}</h3>
            <p>{{ $t(feature.descriptionKey) }}</p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
  import { ref } from 'vue'

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
</script>

<style scoped>
  .features-section {
    background: #f5f5f5;
  }

  .feature-item {
    height: 100%;
    transition: transform 0.3s;
  }

  .feature-item:hover {
    transform: translateY(-10px);
  }
</style>
