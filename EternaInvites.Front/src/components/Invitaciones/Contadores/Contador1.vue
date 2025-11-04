<template>
  <Countdown
    :labels="{
      days: $t('contador.dias'),
      hours: $t('contador.horas'),
      minutes: $t('contador.minutos'),
      seconds: $t('contador.segundos'),
    }"
    :mainFlipBackgroundColor="options.mainFlipBackgroundColor"
    :secondFlipBackgroundColor="options.secondFlipBackgroundColor"
    :mainColor="options.mainColor"
    :secondFlipColor="options.secondFlipColor"
    :deadlineDate="new Date(2025, 11, 31, 23, 59, 59, 0)" />
</template>

<script setup lang="ts">
  import { Countdown } from 'vue3-flip-countdown'

  interface CountdownOptions {
    mainFlipBackgroundColor?: string
    secondFlipBackgroundColor?: string
    mainColor?: string
    secondFlipColor?: string
    deadline?: Date
  }

  const {
    options = {
      deadline: new Date('2025-11-15T00:00:00'),
      mainFlipBackgroundColor: '#f0f0f0',
      secondFlipBackgroundColor: '#f0f0f0',
      mainColor: 'red',
      secondFlipColor: 'red',
    },
  } = defineProps<{ options?: CountdownOptions }>()
</script>

<style lang="css">
  .flip-card__bottom {
    border-top: 0px !important;
  }
  .flip-card__back-bottom {
    border-top: 0px !important;
  }
</style>
