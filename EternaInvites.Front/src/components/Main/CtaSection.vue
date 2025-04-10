<template>
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
</template>

<script setup lang="ts">
  // No additional script logic needed for this component
</script>

<style scoped>
  /* CTA Section */
  .cta-section {
    background-image:
      linear-gradient(rgba(0, 0, 0, 0.6), rgba(0, 0, 0, 0.6)),
      url('https://images.unsplash.com/photo-1469371670807-013ccf25f16a?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80');
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
  }
</style>
