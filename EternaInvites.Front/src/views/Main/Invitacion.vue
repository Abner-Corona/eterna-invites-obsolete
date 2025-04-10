<template>
  <div>
    a

    <Suspense>
      <template #default>
        <component :is="Petalos" />
      </template>
      <template #fallback>
        <div class="loading">Loading...</div>
      </template>
    </Suspense>
  </div>
</template>
<script setup lang="ts">
  const Petalos = defineAsyncComponent(() => import('src/components/Petalos.vue'))
</script>
