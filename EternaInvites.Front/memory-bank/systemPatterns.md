# System Patterns - Eterna Invites

## Overall Architecture

### Application Structure
```
Frontend (Quasar/Vue 3)
├── Public Pages (Invitations & Demos)
├── Admin Dashboard (Protected)
└── Shared Components & Services

Backend Services (External)
├── Authentication API
├── Templates API
└── Client Management API
```

## Design Patterns

### 1. Component Composition Pattern
**Purpose**: Build complex invitation interfaces from reusable components

**Implementation**:
```typescript
// Async component loading for performance
const AudioPlayer1 = defineAsyncComponent(
  () => import('src/components/Invitaciones/AudioPlayers/AudioPlayer1.vue')
)
const Contador1 = defineAsyncComponent(
  () => import('src/components/Invitaciones/Contadores/Contador1.vue')
)
```

**Benefits**:
- Code splitting and lazy loading
- Reusable across different invitation templates
- Easy maintenance and updates

### 2. Service Layer Pattern
**Purpose**: Centralize API communication and data transformation

**Structure**:
```
src/services/
├── LoginService.ts           # Authentication
├── PlantillasService.ts      # Template management
└── Clientes/
    ├── ClientesService.ts    # Client management
    └── ObtenerInvitacion.ts  # Invitation retrieval
```

**Benefits**:
- Separation of concerns
- Type-safe API interactions
- Centralized error handling

### 3. Store Pattern (Pinia)
**Purpose**: Global state management with persistence

**Implementation**:
```typescript
export const useAuthStore = defineStore('authStore', {
  state: (): AuthStoreStoreState => ({
    token: '',
  }),
  persist: true, // Automatic persistence
})
```

**Key Stores**:
- `authStore`: Authentication state
- Future: `invitationStore`, `templateStore`

### 4. Route-Based Architecture
**Purpose**: Organize application by user flows and access levels

**Route Structure**:
```typescript
const routes = [
  { path: '/', component: Main },           // Landing page
  { path: '/demo1', component: Demo1 },     // Demo invitations
  { path: '/:id', component: Invitacion }, // Dynamic invitations
  { path: '/login', component: Login },     // Authentication
  { path: '/dashboard/*', component: Dashboard }, // Admin panel
]
```

### 5. Template System Pattern
**Purpose**: Dynamic invitation generation from reusable templates

**Component Types**:
- **Audio Players**: Background music integration
- **Countdown Timers**: Event countdown displays
- **Content Blocks**: Text, images, event details
- **Interactive Elements**: Calendar integration, RSVP forms

## Component Architecture

### Invitation Component Hierarchy
```
Invitation Template
├── Header Section
│   ├── Title Component
│   └── Audio Player
├── Content Section
│   ├── Message Component
│   ├── Date Component
│   └── Countdown Timer
├── Details Section
│   ├── Event Info Component
│   └── Parents/Sponsors Info
└── Action Section
    └── Calendar Integration
```

### Shared Component Library
```
src/components/
├── Lottie.vue                 # Animation wrapper
├── DevTools/                  # Development utilities
├── Invitaciones/              # Invitation-specific components
│   ├── AñadirCalendario.vue   # Calendar integration
│   ├── Imagen.vue             # Image display
│   ├── Petalos.vue            # Decoration animations
│   ├── AudioPlayers/          # Audio player variants
│   ├── Contadores/            # Countdown timer variants
│   └── Itinerarios/           # Event schedule components
└── Main/                      # Landing page components
    ├── CtaSection.vue
    ├── DemosSection.vue
    ├── FeaturesSection.vue
    ├── HeroSection.vue
    └── NavigationMenu.vue
```

## Data Flow Patterns

### 1. Props Down, Events Up
**Implementation**: Parent components pass data down via props, children emit events upward

**Example**:
```vue
<!-- Parent -->
<AudioPlayer1 :option="audioConfig" @play="handlePlay" />

<!-- Child emits events -->
this.$emit('play', { timestamp: Date.now() })
```

### 2. Composition API Pattern
**Purpose**: Organize logic in reusable composables

**Structure**:
```typescript
// Composable for invitation data
export function useInvitationData(invitationId: string) {
  const invitation = ref(null)
  const loading = ref(false)
  
  const fetchInvitation = async () => {
    loading.value = true
    // API call logic
    loading.value = false
  }
  
  return { invitation, loading, fetchInvitation }
}
```

### 3. Async Component Loading
**Purpose**: Performance optimization through code splitting

**Pattern**:
```typescript
// Only load when needed
const HeavyComponent = defineAsyncComponent({
  loader: () => import('./HeavyComponent.vue'),
  loadingComponent: LoadingSpinner,
  errorComponent: ErrorComponent,
  delay: 200,
  timeout: 3000
})
```

## State Management Architecture

### Store Organization
```
stores/
├── index.ts          # Store configuration
├── authStore.ts      # Authentication state
├── templateStore.ts  # Template management (future)
├── clientStore.ts    # Client data (future)
└── uiStore.ts        # UI state (future)
```

### State Persistence Strategy
- **Critical Data**: Authentication tokens, user preferences
- **Session Data**: Current invitation being viewed, UI state
- **Cache Strategy**: Template data, frequently accessed content

## Routing Architecture

### Route Protection Pattern
```typescript
// Authentication guard
beforeEnter: () => {
  const authStore = useAuthStore()
  if (authStore.token) return { path: '/dashboard' }
  else return true
}
```

### Dynamic Route Handling
```typescript
// Invitation routes with ID parameter
{ path: '/:id', component: Invitacion }
// Handles: /abc123, /wedding-2024, etc.
```

## Error Handling Patterns

### 1. Service Layer Error Handling
```typescript
export class ApiService {
  async request(config) {
    try {
      return await api(config)
    } catch (error) {
      // Centralized error logging
      console.error('API Error:', error)
      throw new ServiceError(error.message)
    }
  }
}
```

### 2. Component Error Boundaries
- Vue's `errorCaptured` hook for graceful degradation
- Loading states for async operations
- Fallback UI for failed components

## Performance Patterns

### 1. Lazy Loading Strategy
- Route-based code splitting
- Component-level lazy loading
- Asset lazy loading (images, fonts)

### 2. Caching Strategy
- API response caching
- Component instance reuse
- Static asset caching

### 3. Animation Optimization
- CSS transforms over position changes
- RequestAnimationFrame for custom animations
- Lottie for complex vector animations

## Integration Patterns

### 1. Third-Party Service Integration
```typescript
// Calendar integration
export class CalendarService {
  static generateGoogleCalendarUrl(event: EventDetails): string
  static generateICSFile(event: EventDetails): Blob
  static generateOutlookUrl(event: EventDetails): string
}
```

### 2. Media Integration
- External audio streaming
- Responsive image loading
- Font loading optimization

### 3. Analytics Integration
- Event tracking for user interactions
- Performance monitoring
- Error reporting

## Security Patterns

### 1. Authentication Flow
```
1. User logs in → Token stored in Pinia
2. Token persisted → Survives page refresh
3. Protected routes → Check token validity
4. API calls → Include token in headers
```

### 2. Input Validation
- TypeScript interfaces for type safety
- Prop validation in components
- API response validation

### 3. XSS Prevention
- Vue's automatic escaping
- Sanitized HTML content
- Safe dynamic content rendering
