<template>
  <div class="petalos" :class="{ 'reverse-direction': direction === 'left' }">
    <div
      v-for="n in petalCount"
      :key="n"
      class="petal"
      :style="{
        '--primary-color': primaryColor,
        '--secondary-color': secondaryColor,
        'animation-delay': `${(n * 1.25) % 10}s`,
        top: `${(n * 8) % 100}%`,
      }"></div>
  </div>
</template>

<script setup lang="ts">
  const props = defineProps({
    // Color of the petals
    primaryColor: {
      type: String,
      default: '#fe85c0',
    },
    secondaryColor: {
      type: String,
      default: '#db0637',
    },
    // Number of petals to display
    petalCount: {
      type: Number,
      default: 12,
    },
    // Direction of movement ('right', 'left', 'arriba', or 'bajo')
    direction: {
      type: String,
      default: 'left',
      validator: (value: string) => ['right', 'left', 'arriba', 'bajo'].includes(value),
    },
  })
</script>

<style lang="scss">
  .petalos {
    position: absolute;
    left: -20px;
    top: 0;
    right: 0;
    bottom: 0;
    height: 100vh;
    overflow: hidden;
    perspective: 1000px;
  }

  .petal {
    margin: 1px;
    animation: fall1 10s linear infinite;
    position: absolute;
    top: 0;
    left: 0;

    background: var(--primary-color, #fe85c0);
    width: 10px;
    height: 10px;
    box-shadow: inset 3px 3px 5px -3px #fff;
    border-radius: 15px 5px 15px 0px;
    backface-visibility: hidden;
    transform-style: preserve-3d;
  }

  .petal::after {
    content: '';
    right: 0px;
    bottom: 0px;
    position: absolute;
    top: 0px;
    left: 0px;
    background: var(--secondary-color, #db0637);
    border-radius: 15px 5px 15px 0px;
    transform: rotateY(180deg);
    transform-style: preserve-3d;
    backface-visibility: hidden;
  }

  .petal:nth-child(2n) {
    animation-name: fall2;
  }
  .petal:nth-child(3n) {
    animation-name: fall3;
  }
  .petal:nth-child(4n) {
    animation-name: fall4;
  }
  .petal:nth-child(5n) {
    animation-name: fall5;
  }
  .petal:nth-child(6n) {
    animation-name: fall6;
  }

  .reverse-direction .petal {
    animation-direction: reverse;
  }

  @keyframes fall1 {
    to {
      transform: translateY(200px) translateX(2560px) translateZ(200px) rotate3d(1, 0.25, 0, 320deg);
    }
  }

  @keyframes fall2 {
    to {
      transform: translateY(150px) translateX(2560px) translateZ(500px)
        rotate3d(1, 0.25, 0.25, 220deg);
    }
  }

  @keyframes fall3 {
    to {
      transform: translateY(150px) translateX(2560px) translateZ(300px) rotate3d(0.25, 1, 1, 420deg);
    }
  }

  @keyframes fall4 {
    to {
      transform: translateY(-160px) translateX(2560px) translateZ(-500px)
        rotate3d(1, 1, 0.25, 720deg);
    }
  }

  @keyframes fall5 {
    to {
      transform: translateY(-200px) translateX(2560px) translateZ(400px)
        rotate3d(0.25, 1, 0.25, 820deg);
    }
  }

  @keyframes fall6 {
    to {
      transform: translateY(-300px) translateX(2560px) translateZ(-100px) rotate3d(1, 1, 1, 720deg);
    }
  }
</style>
