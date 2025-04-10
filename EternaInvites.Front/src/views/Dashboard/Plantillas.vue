<template>
  <div class="q-pa-md">
    <div class="row justify-between q-mb-md">
      <h2 class="text-h4 q-my-none">Plantillas</h2>
      <q-btn color="primary" icon="add" label="Nueva Plantilla" @click="openDialog()" />
    </div>

    <!-- Search and filters -->
    <div class="row q-mb-md">
      <q-input
        v-model="search"
        debounce="300"
        outlined
        placeholder="Buscar plantillas..."
        class="col-12 col-md-6">
        <template v-slot:append>
          <q-icon name="search" />
        </template>
      </q-input>
    </div>

    <!-- Table with loading state -->
    <q-table
      :rows="filteredPlantillas"
      :columns="columns"
      row-key="id"
      :loading="loading"
      :pagination="pagination"
      binary-state-sort>
      <template v-slot:loading>
        <q-inner-loading showing color="primary" />
      </template>

      <template v-slot:body-cell-mostrarDemo="props">
        <q-td :props="props">
          <q-chip
            :color="props.row.mostrarDemo ? 'positive' : 'grey'"
            text-color="white"
            :label="props.row.mostrarDemo ? 'Visible' : 'Oculto'"
            size="sm" />
        </q-td>
      </template>

      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <div class="q-gutter-sm">
            <q-btn
              icon="visibility"
              color="info"
              dense
              size="sm"
              round
              @click="previewPlantilla(props.row)" />
            <q-btn
              icon="edit"
              color="amber"
              dense
              size="sm"
              round
              @click="editPlantilla(props.row)" />
            <q-btn
              icon="delete"
              color="negative"
              dense
              size="sm"
              round
              @click="confirmDelete(props.row)" />
          </div>
        </q-td>
      </template>
    </q-table>

    <!-- Dialog for add/edit -->
    <q-dialog v-model="dialog" persistent maximized>
      <q-card>
        <q-card-section class="row items-center bg-primary text-white">
          <div class="text-h6">{{ editMode ? 'Editar Plantilla' : 'Nueva Plantilla' }}</div>
          <q-space />
          <q-btn icon="close" flat round dense v-close-popup />
        </q-card-section>

        <q-card-section class="q-pt-md">
          <q-form @submit="savePlantilla" class="q-gutter-md">
            <!-- Información Básica -->
            <div class="text-subtitle1 q-mb-sm">Información Básica</div>
            <div class="row q-col-gutter-md">
              <div class="col-12 col-md-6">
                <q-input
                  v-model="currentPlantilla.nombre"
                  label="Nombre *"
                  :rules="[(val) => !!val || 'Nombre es requerido']"
                  outlined
                  dense />
              </div>
              <div class="col-12 col-md-6">
                <q-input
                  v-model="currentPlantilla.categoria"
                  label="Categoría *"
                  :rules="[(val) => !!val || 'Categoría es requerida']"
                  outlined
                  dense />
              </div>
            </div>

            <div class="row q-col-gutter-md">
              <div class="col-12">
                <q-toggle v-model="currentPlantilla.mostrarDemo" label="Mostrar en demos" />
              </div>
            </div>

            <!-- HTML Editor -->
            <div class="text-subtitle1 q-mb-sm q-mt-md">Código HTML</div>
            <div class="row q-col-gutter-md">
              <div class="col-12">
                <q-input
                  v-model="currentPlantilla.html"
                  type="textarea"
                  filled
                  autogrow
                  label="HTML"
                  style="font-family: monospace"
                  rows="10" />
              </div>
            </div>

            <!-- Componentes -->
            <div class="text-subtitle1 q-mb-sm q-mt-md">
              Componentes
              <q-btn
                label="Añadir Componente"
                color="secondary"
                dense
                class="q-ml-sm"
                @click="addComponent" />
            </div>

            <!-- Componentes list -->
            <q-list bordered separator>
              <q-expansion-item
                v-for="(componente, index) in currentPlantilla.componentes"
                :key="index"
                :label="`Componente: ${componente.nombre}`"
                header-class="text-primary">
                <q-card>
                  <q-card-section>
                    <div class="row q-col-gutter-md">
                      <div class="col-12 col-md-4">
                        <q-input v-model="componente.nombre" label="Nombre" outlined dense />
                      </div>
                      <div class="col-12 col-md-4">
                        <q-input v-model="componente.tipo" label="Tipo" outlined dense />
                      </div>
                      <div class="col-12 col-md-4">
                        <q-input v-model="componente.id" label="ID" outlined dense />
                      </div>
                    </div>

                    <div class="row q-col-gutter-md q-mt-sm">
                      <div class="col-12">
                        <q-input
                          v-model="componente.contenido"
                          label="Contenido"
                          type="textarea"
                          outlined
                          autogrow
                          dense />
                      </div>
                    </div>

                    <!-- Component actions -->
                    <div class="row justify-end q-mt-sm">
                      <q-btn
                        color="negative"
                        label="Eliminar"
                        dense
                        flat
                        @click="removeComponent(index)" />
                    </div>
                  </q-card-section>
                </q-card>
              </q-expansion-item>
            </q-list>

            <div class="row justify-end q-mt-lg">
              <q-btn label="Cancelar" color="grey" flat v-close-popup class="q-mr-sm" />
              <q-btn label="Guardar" type="submit" color="primary" />
            </div>
          </q-form>
        </q-card-section>
      </q-card>
    </q-dialog>

    <!-- Confirmation dialog for delete -->
    <q-dialog v-model="deleteDialog" persistent>
      <q-card>
        <q-card-section class="row items-center">
          <q-avatar icon="warning" color="negative" text-color="white" />
          <span class="q-ml-sm">¿Está seguro de eliminar esta plantilla?</span>
        </q-card-section>

        <q-card-actions align="right">
          <q-btn flat label="Cancelar" color="primary" v-close-popup />
          <q-btn flat label="Eliminar" color="negative" @click="deletePlantilla" v-close-popup />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <!-- Preview dialog -->
    <q-dialog v-model="previewDialog" maximized>
      <q-card>
        <q-card-section class="row items-center bg-primary text-white">
          <div class="text-h6">Vista previa: {{ previewPlantillaData?.nombre }}</div>
          <q-space />
          <q-btn icon="close" flat round dense v-close-popup />
        </q-card-section>

        <q-card-section>
          <div class="preview-container" v-if="previewPlantillaData">
            <iframe
              :srcdoc="previewPlantillaData.html"
              style="width: 100%; height: 70vh; border: 1px solid #ddd"></iframe>
          </div>
        </q-card-section>
      </q-card>
    </q-dialog>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, computed } from 'vue'
  import { QTable, useQuasar } from 'quasar'
  import { PlantillasModel, Componente, PlantillasService } from 'src/services/PlantillasService'

  const $q = useQuasar()

  // Table data
  const plantillas = ref<PlantillasModel[]>([])
  const loading = ref(false)
  const search = ref('')
  const pagination = ref({
    sortBy: 'nombre',
    descending: false,
    page: 1,
    rowsPerPage: 10,
    rowsNumber: 0,
  })

  // Dialog controls
  const dialog = ref(false)
  const deleteDialog = ref(false)
  const previewDialog = ref(false)
  const editMode = ref(false)
  const currentPlantilla = ref<PlantillasModel>({
    id: 0,
    uui: '',
    nombre: '',
    mostrarDemo: true,
    html: '',
    componentes: [],
    categoria: '',
  })
  const plantillaToDelete = ref<PlantillasModel | null>(null)
  const previewPlantillaData = ref<PlantillasModel | null>(null)

  // Table columns definition
  const columns: QTable['columns'] = [
    {
      name: 'nombre',
      required: true,
      label: 'Nombre',
      align: 'left',
      field: 'nombre',
      sortable: true,
    },
    { name: 'categoria', align: 'left', label: 'Categoría', field: 'categoria', sortable: true },
    {
      name: 'mostrarDemo',
      align: 'center',
      label: 'Estado',
      field: 'mostrarDemo',
    },
    {
      name: 'componentes',
      align: 'center',
      label: 'Componentes',
      field: (row) => row.componentes?.length || 0,
    },
    { name: 'actions', align: 'center', label: 'Acciones', field: 'actions' },
  ]

  // Filtered plantillas based on search
  const filteredPlantillas = computed(() => {
    if (!search.value) return plantillas.value

    const searchLower = search.value.toLowerCase()
    return plantillas.value.filter(
      (plantilla) =>
        plantilla.nombre.toLowerCase().includes(searchLower) ||
        plantilla.categoria.toLowerCase().includes(searchLower),
    )
  })

  // Load plantillas on component mount
  onMounted(async () => {
    await fetchPlantillas()
  })

  // Fetch plantillas from API
  async function fetchPlantillas() {
    loading.value = true
    try {
      plantillas.value = await PlantillasService.GetAll()
      pagination.value.rowsNumber = plantillas.value.length
    } catch (error) {
      $q.notify({
        color: 'negative',
        position: 'top',
        message: 'Error al cargar las plantillas',
        icon: 'report_problem',
      })
    } finally {
      loading.value = false
    }
  }

  // Dialog functions
  function openDialog(plantilla?: PlantillasModel) {
    if (plantilla) {
      currentPlantilla.value = JSON.parse(JSON.stringify(plantilla)) // Deep clone
      editMode.value = true
    } else {
      currentPlantilla.value = {
        id: 0,
        uui: crypto.randomUUID(),
        nombre: '',
        mostrarDemo: true,
        html: '',
        componentes: [],
        categoria: '',
      }
      editMode.value = false
    }
    dialog.value = true
  }

  function editPlantilla(plantilla: PlantillasModel) {
    openDialog(plantilla)
  }

  function previewPlantilla(plantilla: PlantillasModel) {
    previewPlantillaData.value = plantilla
    previewDialog.value = true
  }

  function confirmDelete(plantilla: PlantillasModel) {
    plantillaToDelete.value = plantilla
    deleteDialog.value = true
  }

  // Add new component to the template
  function addComponent() {
    const newComponent: Componente = {
      id: crypto.randomUUID(),
      nombre: 'Nuevo Componente',
      tipo: 'div',
      contenido: '',
      propiedades: { additionalProp1: '', additionalProp2: '', additionalProp3: '' },
      clases: [],
      estilos: [],
      hijos: [],
      eventos: { additionalProp1: '', additionalProp2: '', additionalProp3: '' },
    }
    currentPlantilla.value.componentes.push(newComponent)
  }

  // Remove component from the template
  function removeComponent(index: number) {
    currentPlantilla.value.componentes.splice(index, 1)
  }

  // CRUD operations
  async function savePlantilla() {
    loading.value = true
    try {
      if (editMode.value && currentPlantilla.value.id) {
        await PlantillasService.Update(currentPlantilla.value)
        $q.notify({
          color: 'positive',
          position: 'top',
          message: 'Plantilla actualizada correctamente',
          icon: 'check',
        })
      } else {
        await PlantillasService.Insert(currentPlantilla.value)
        $q.notify({
          color: 'positive',
          position: 'top',
          message: 'Plantilla creada correctamente',
          icon: 'check',
        })
      }
      dialog.value = false
      await fetchPlantillas()
    } catch (error) {
      $q.notify({
        color: 'negative',
        position: 'top',
        message: 'Error al guardar la plantilla',
        icon: 'report_problem',
      })
    } finally {
      loading.value = false
    }
  }

  async function deletePlantilla() {
    if (!plantillaToDelete.value || !plantillaToDelete.value.id) return

    loading.value = true
    try {
      await PlantillasService.Delete(plantillaToDelete.value.id)
      $q.notify({
        color: 'positive',
        position: 'top',
        message: 'Plantilla eliminada correctamente',
        icon: 'check',
      })
      await fetchPlantillas()
    } catch (error) {
      $q.notify({
        color: 'negative',
        position: 'top',
        message: 'Error al eliminar la plantilla',
        icon: 'report_problem',
      })
    } finally {
      loading.value = false
      plantillaToDelete.value = null
    }
  }
</script>

<style scoped>
  .q-table__card {
    box-shadow: 0 1px 5px rgba(0, 0, 0, 0.2);
    border-radius: 4px;
  }

  .preview-container {
    background-color: #f5f5f5;
    padding: 20px;
    border-radius: 8px;
  }
</style>
