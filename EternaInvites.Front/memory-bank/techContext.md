# Technical Context - Eterna Invites

## Technology Stack

### Frontend Framework
- **Vue.js 3.5.13**: Main framework with Composition API
- **Quasar 2.18.1**: Vue.js framework for building responsive applications
- **TypeScript**: Type-safe development
- **Vite**: Build tool and development server

### State Management & Routing
- **Pinia 3.0.2**: State management with persistence plugin
- **Vue Router 4.5.0**: Client-side routing
- **Pinia Persistedstate**: State persistence across sessions

### UI & Styling
- **Quasar Components**: UI component library
- **SCSS**: CSS preprocessing
- **Responsive Design**: Mobile-first approach
- **Custom Fonts**: Special wedding fonts in `src/assets/fuentes/corazones/`

### Multimedia & Animations
- **Lottie Web 5.12.2**: Vector animations
- **@lottiefiles/dotlottie-vue**: Vue Lottie integration
- **Motion-v**: Animation library for Vue
- **vue3-flip-countdown**: Countdown timer component

### Development Tools
- **OXLint**: Fast JavaScript/TypeScript linter
- **Prettier**: Code formatting
- **ESLint**: Additional linting with plugins
- **Vitest**: Unit testing framework
- **Cypress**: E2E and component testing

### Utilities & Services
- **Axios 1.8.4**: HTTP client for API calls
- **VueUse 13.1.0**: Vue composition utilities
- **Vue-i18n 11.1.3**: Internationalization support

## Project Structure

### Core Directories
```
src/
├── assets/           # Static assets (fonts, images)
├── boot/            # Quasar boot files (axios, i18n, vueuse)
├── components/      # Reusable Vue components
├── css/            # Global styles and Quasar variables
├── i18n/           # Internationalization files (es.json, en.json)
├── router/         # Vue Router configuration
├── services/       # API service layer
├── stores/         # Pinia stores
└── views/          # Page components
```

### Key Component Categories
- **Main Components**: Landing page, demos, invitations
- **Dashboard Components**: Admin panel for management
- **Invitation Components**: Audio players, countdown timers, calendar integration
- **Shared Components**: Reusable UI elements

## Development Environment

### Build & Development
- **Package Manager**: Bun (fast package manager)
- **Dev Server**: `quasar dev` (hot reload enabled)
- **Production Build**: `quasar build`
- **Linting**: Automatic on save with OXLint

### Code Quality Tools
- **Linting Strategy**: OXLint for performance, ESLint for additional rules
- **Formatting**: Prettier with automatic formatting on save
- **Type Checking**: TypeScript with strict configuration

### Testing Setup
- **Unit Tests**: Vitest for component testing
- **E2E Tests**: Cypress for full application testing
- **Component Tests**: Cypress component testing

## Architecture Patterns

### Component Architecture
- **Composition API**: Modern Vue 3 patterns
- **Async Components**: Lazy loading for performance
- **Props/Events**: Clear component communication
- **Slot-based**: Flexible component composition

### State Management
- **Pinia Stores**: Modular state management
- **Persistence**: State persisted across sessions
- **Type Safety**: TypeScript interfaces for store state

### Routing Strategy
- **Dynamic Routes**: Invitation IDs as route parameters (`/:id`)
- **Route Guards**: Authentication protection for dashboard
- **Lazy Loading**: Route-based code splitting

## API Integration

### Service Layer Pattern
```typescript
// Example service structure
export interface PlantillasModel {
  id: number
  uui: string
  nombre: string
  mostrarDemo: boolean
  html: string
  componentes: Componente[]
  categoria: string
}
```

### Authentication
- **Token-based**: JWT tokens stored in Pinia
- **Route Protection**: Dashboard routes require authentication
- **Persistent Sessions**: Token persistence across browser sessions

## Performance Considerations

### Optimization Strategies
- **Lazy Loading**: Components and routes loaded on demand
- **Asset Optimization**: Quasar's built-in optimizations
- **Bundle Splitting**: Automatic code splitting
- **Tree Shaking**: Unused code elimination

### Multimedia Optimization
- **Lottie Animations**: Lightweight vector animations
- **Audio Streaming**: External audio sources
- **Image Optimization**: Responsive image loading

## Development Constraints

### Browser Support
- **Modern Browsers**: ES2020+ features
- **Mobile-First**: Primary focus on mobile devices
- **Cross-Platform**: iOS, Android, Desktop

### Performance Targets
- **Initial Load**: < 3 seconds
- **Interactive**: < 1 second after load
- **Animations**: 60fps smooth animations

### Technical Limitations
- **Client-Side Only**: No server-side rendering currently
- **External Dependencies**: Audio and some assets hosted externally
- **Build Size**: Keep bundle size optimized for mobile

## Security Considerations
- **Input Validation**: TypeScript interfaces for type safety
- **XSS Prevention**: Vue's built-in protection
- **Token Storage**: Secure token management in stores
- **HTTPS Only**: Production deployment requirements

## Development Workflow
1. **Code Style**: Automatic formatting and linting on save
2. **Component Development**: Composition API with TypeScript
3. **Testing**: Unit tests for components, E2E for user flows
4. **Build Process**: Quasar CLI for development and production builds
