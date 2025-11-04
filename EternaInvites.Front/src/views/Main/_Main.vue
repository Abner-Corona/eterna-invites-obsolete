<template>
  <q-layout view="lHh lpR lFf" class="no-header-padding">
    <!-- Header Component -->
    <Header />

    <!-- Mobile Navigation Component (at layout level) -->
    <MobileNavigation />

    <!-- Main Content -->
    <q-page-container>
      <q-page class="no-padding">
        <Container1></Container1>
        <Container2></Container2>
      </q-page>
    </q-page-container>
  </q-layout>
</template>
<script setup lang="ts">
  const Header = defineAsyncComponent(() => import('src/components/Main/Header/Header.vue'))
  const MobileNavigation = defineAsyncComponent(() => import('src/components/Main/MobileNavigation/MobileNavigation.vue'))
  const Container1 = defineAsyncComponent(() => import('src/components/Main/Container1/Container1.vue'))
  const Container2 = defineAsyncComponent(() => import('src/components/Main/Container2/Container2.vue'))
</script>
<style lang="scss" src="./_Main.scss" scoped></style>
