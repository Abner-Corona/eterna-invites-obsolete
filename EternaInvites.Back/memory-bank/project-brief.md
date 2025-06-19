# EternaInvites - Project Brief

## Overview

EternaInvites is a digital invitation management system that allows clients to create and share customizable event invitations through unique URLs. The system provides template-based invitation generation with HTML/CSS/JS customization capabilities.

## Goals

- **Primary Goal**: Provide a platform for creating and managing digital event invitations
- **Secondary Goals**:
  - Enable template customization through HTML components
  - Provide unique URL access for each client's invitation
  - Support multiple event types and categories
  - Maintain user authentication and authorization

## Scope

### In Scope:

- Client management system with unique URL generation
- Template management with HTML/CSS/JS customization
- User authentication and authorization
- RESTful API for invitation retrieval
- Database persistence with Entity Framework Core
- JWT-based security

### Out of Scope (for now):

- Frontend application (this is backend only)
- Email sending functionality
- Payment processing
- Advanced analytics

## Success Criteria

1. Clients can be created with unique URLs
2. Templates can be managed with custom HTML content
3. Invitations can be retrieved via public URLs
4. System maintains data integrity and security
5. API is properly documented and testable
6. Authentication system works correctly

## Key Features

- **Client Management**: Create clients with event details and unique URLs
- **Template System**: Manage HTML templates with component-based customization
- **Invitation Delivery**: Serve invitations via unique URLs without authentication
- **User Management**: Secure admin access with JWT authentication
- **Data Persistence**: MySQL database with Entity Framework Core migrations

## Target Users

- **Primary**: System administrators who manage clients and templates
- **Secondary**: End users (invitation recipients) who access invitations via URLs
