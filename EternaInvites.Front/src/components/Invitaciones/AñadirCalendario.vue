<template>
  <button class="calendar-button" @click="addToCalendar" aria-label="Añadir al calendario">
    <i class="calendar-icon"></i>
    <span>Añadir al calendario</span>
  </button>
</template>

<script setup lang="ts">
  interface EventDetails {
    title: string
    description: string
    location: string
    startDate: string
    endDate: string
  }

  const props = defineProps<{
    eventDetails: EventDetails
  }>()

  // Función para añadir al calendario
  const addToCalendar = () => {
    // Formatear las fechas para Google Calendar
    const startDate = props.eventDetails.startDate.replace(/-|:|\.\d+/g, '')
    const endDate = props.eventDetails.endDate.replace(/-|:|\.\d+/g, '')

    // Crear la URL de Google Calendar
    const googleCalendarUrl =
      `https://calendar.google.com/calendar/render?action=TEMPLATE` +
      `&text=${encodeURIComponent(props.eventDetails.title)}` +
      `&dates=${startDate}/${endDate}` +
      `&details=${encodeURIComponent(props.eventDetails.description)}` +
      `&location=${encodeURIComponent(props.eventDetails.location)}` +
      `&sf=true` +
      `&output=xml`

    // Abrir Google Calendar en una nueva ventana
    window.open(googleCalendarUrl, '_blank')
  }
</script>

<style scoped>
  .calendar-button {
    position: fixed;
    bottom: 20px;
    right: 20px;
    padding: 12px 20px;
    background-color: #a70000;
    color: white;
    border: none;
    border-radius: 50px;
    font-size: 1rem;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
    display: flex;
    align-items: center;
    gap: 8px;
    cursor: pointer;
    z-index: 1000;
    transition:
      transform 0.2s,
      background-color 0.3s;
  }

  .calendar-button:hover {
    transform: translateY(-2px);
    background-color: #8e0000;
  }

  .calendar-button:active {
    transform: translateY(0);
  }

  .calendar-icon {
    display: inline-block;
    width: 20px;
    height: 20px;
    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='white'%3E%3Cpath d='M19,3H18V1H16V3H8V1H6V3H5A2,2 0 0,0 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5A2,2 0 0,0 19,3M19,19H5V8H19V19M12,10V12H10V16H14V14H12V10Z'/%3E%3C/svg%3E");
    background-size: contain;
    background-repeat: no-repeat;
  }

  /* Media query para dispositivos móviles */
  @media (max-width: 768px) {
    .calendar-button {
      padding: 10px 16px;
      font-size: 0.9rem;
    }

    .calendar-button span {
      display: none; /* Ocultar texto en móviles, solo mostrar icono */
    }

    .calendar-icon {
      margin: 0;
    }
  }
</style>
