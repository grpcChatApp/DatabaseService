-- SEED SCOPES
INSERT INTO auth.scopes (name, display_name, description)
VALUES
    -- Flutter Client Scopes (What the User App does)
    ('chat.message.send', 'Send Messages', 'Allows the flutter app to send chat messages'),
    ('chat.history.read', 'Read History', 'Allows the flutter app to view past conversations'),
    ('profile.manage',    'Manage Profile', 'Allows the user to update their own display name/photo'),

    -- Backend Host Service Scopes (Internal logic capabilities)
    ('identity.user.manage', 'User Management', 'Allows creating and deleting users (Admin Only)'),
    ('identity.role.assign', 'Assign Roles', 'Allows modifying user roles'),

    -- Broker Service Scopes (Infrastructure)
    ('broker.event.publish', 'Publish Events', 'Allows the backend to push events to Kafka'),
    ('broker.queue.manage',  'Manage Queues', 'Allows creating/deleting Kafka topics'),

    -- Database Service Scopes (Direct DB Access)
    ('db.schema.modify',  'Modify Schema', 'Allows running DDL/Migrations'),
    ('db.data.write',     'Write Data', 'Allows standard CRUD operations'),
    ('db.data.read',      'Read Data', 'Allows read-only access')
ON CONFLICT (name) DO NOTHING;

INSERT INTO auth.permissions (name, display_name, description)
VALUES
    ('message:create', 'Create Message', 'Allows sending new messages'),
    ('message:read', 'Read Messages', 'Allows viewing chat history'),
    ('message:delete', 'Delete Message', 'Allows removing any message (Admin)'),
    ('user:create', 'Create User', 'Allows registering new users'),
    ('user:delete', 'Delete User', 'Allows removing user accounts'),
    ('topic:publish', 'Publish to Topic', 'Allows pushing data to Kafka/Event bus')
ON CONFLICT (name) DO NOTHING;


INSERT INTO auth.resources (name, display_name)
VALUES 
    ('chat-api', 'Chat gRPC Service'),
    ('broker-internal', 'Kafka Internal Service'),
    ('database-internal', 'Postgres Internal Service')
ON CONFLICT (name) DO NOTHING;

INSERT INTO auth.clients (name, client_id, client_secret, type)
VALUES 
    ('Flutter Mobile App', 'flutter-client', '', 'Public'), -- No secret for mobile
    ('Host Backend Service', 'host-service', 'host-secret-2025', 'Confidential'),
    ('Database Service', 'db-service', 'db-secret-2025', 'Confidential')
ON CONFLICT (client_id) DO NOTHING;

INSERT INTO auth.client_resource_mappings (client_id, resource_id)
SELECT c.id, r.id FROM auth.clients c, auth.resources r
WHERE c.client_id = 'flutter-client' AND r.name = 'chat-api'
ON CONFLICT DO NOTHING;

-- Host talks to Database
INSERT INTO auth.client_resource_mappings (client_id, resource_id)
SELECT c.id, r.id FROM auth.clients c, auth.resources r
WHERE c.client_id = 'host-service' AND r.name = 'database-internal'
ON CONFLICT DO NOTHING;

-- Database talks to Broker
INSERT INTO auth.client_resource_mappings (client_id, resource_id)
SELECT c.id, r.id FROM auth.clients c, auth.resources r
WHERE c.client_id = 'db-service' AND r.name = 'broker-internal'
ON CONFLICT DO NOTHING;

INSERT INTO auth.resource_scope_mappings (resource_id, scope_id)
SELECT r.id, s.id FROM auth.resources r, auth.scopes s
WHERE 
    r.name = 'database-internal' 
    AND (
        s.name LIKE 'db.%'
        OR 
        s.name LIKE 'broker.%'
    )
ON CONFLICT DO NOTHING;

INSERT INTO auth.roles (name, description, is_active)
VALUES 
    ('Admin', 'Full system access including user and role management.', true),
    ('User', 'Standard access to application features.', true),
    ('Guest', 'Read-only access to public resources.', true)
ON CONFLICT (name) DO NOTHING;

INSERT INTO auth.role_permission_mappings (role_id, permission_id) 
SELECT 
    r.id,
    p.id 
FROM auth.roles r, auth.permissions p
WHERE r.name = 'Admin' 
  AND p.name IN ('user:create', 'user:delete', 'chat.message.send', 'message:create', 'message:read')
ON CONFLICT DO NOTHING;

INSERT INTO auth.role_permission_mappings (role_id, permission_id) 
SELECT 
    r.id,
    p.id 
FROM auth.roles r, auth.permissions p
WHERE r.name = 'User' 
  AND p.name IN ('message:create', 'message:read')
ON CONFLICT DO NOTHING;

INSERT INTO auth.role_permission_mappings (role_id, permission_id) 
SELECT 
    r.id,
    p.id 
FROM auth.roles r, auth.permissions p
WHERE r.name = 'Guest' 
  AND p.name IN ('message:read')
ON CONFLICT DO NOTHING;

