CREATE TABLE IF NOT EXISTS auth.scopes (
    id SERIAL         PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    display_name VARCHAR(100) NOT NULL,
    description TEXT,
    reference_id      UUID DEFAULT gen_random_uuid() NOT NULL,
    is_active         BOOLEAN NOT NULL DEFAULT TRUE,
    created_date      TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date      TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX idx_scopes_name ON auth.scopes(name);

CREATE TABLE IF NOT EXISTS auth.users (
    id SERIAL         PRIMARY KEY,
    name TEXT         NOT NULL,
    reference_id UUID DEFAULT gen_random_uuid() NOT NULL,
    username TEXT     NOT NULL UNIQUE,
    email TEXT        NOT NULL UNIQUE,
    password_hash     TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX idx_users_name ON auth.users(name);

CREATE TABLE IF NOT EXISTS auth.roles (
    id SERIAL         PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    description TEXT,
    reference_id UUID DEFAULT gen_random_uuid(),
    is_active BOOLEAN NOT NULL DEFAULT true,
    created_date TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
CREATE INDEX idx_roles_name ON auth.roles(name);

CREATE TABLE IF NOT EXISTS auth.clients (
    id              SERIAL PRIMARY KEY,
    name            TEXT NOT NULL,
    reference_id    UUID DEFAULT gen_random_uuid() NOT NULL,
    client_id       VARCHAR(100) NOT NULL UNIQUE,
    client_secret   VARCHAR(100) NOT NULL,
    type            VARCHAR(25) NOT NULL,
    is_active       BOOLEAN NOT NULL DEFAULT TRUE,
    created_date    TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date    TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX idx_clients_name ON auth.clients(name);

CREATE TABLE auth.permissions (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    display_name VARCHAR(100) NOT NULL,
    description TEXT,
    created_date    TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date    TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX idx_permissions_name ON auth.permissions(name);


CREATE TABLE IF NOT EXISTS auth.resources (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    display_name VARCHAR(100) NOT NULL,
    reference_id UUID DEFAULT gen_random_uuid() NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX idx_resources_name ON auth.resources(name);

CREATE TABLE IF NOT EXISTS auth.client_resource_mappings (
    client_id INT NOT NULL REFERENCES auth.clients(id) ON DELETE CASCADE,
    resource_id INT NOT NULL REFERENCES auth.resources(id) ON DELETE CASCADE,

    CONSTRAINT fk_client_resource_mappings_client FOREIGN KEY (client_id) REFERENCES auth.clients(id) ON DELETE CASCADE,
    CONSTRAINT fk_client_resource_mappings_scope FOREIGN KEY (resource_id) REFERENCES auth.resources(id) ON DELETE CASCADE,
    PRIMARY KEY (client_id, resource_id)
);

CREATE TABLE IF NOT EXISTS auth.resource_scope_mappings (
    resource_id INT NOT NULL REFERENCES auth.resources(id) ON DELETE CASCADE,
    scope_id INT NOT NULL REFERENCES auth.scopes(id) ON DELETE CASCADE,

    CONSTRAINT fk_resource_scope_mappings_client FOREIGN KEY (resource_id) REFERENCES auth.resources(id) ON DELETE CASCADE,
    CONSTRAINT fk_resource_scope_mappings_scope FOREIGN KEY (scope_id) REFERENCES auth.scopes(id) ON DELETE CASCADE,
    PRIMARY KEY (resource_id, scope_id)
);

CREATE TABLE IF NOT EXISTS auth.client_scope_mappings (
    client_id INT NOT NULL REFERENCES auth.clients(id) ON DELETE CASCADE,
    scope_id INT NOT NULL REFERENCES auth.scopes(id) ON DELETE CASCADE,

    CONSTRAINT fk_client_scope_mappings_client FOREIGN KEY (client_id) REFERENCES auth.clients(id) ON DELETE CASCADE,
    CONSTRAINT fk_client_scope_mappings_scope FOREIGN KEY (scope_id) REFERENCES auth.scopes(id) ON DELETE CASCADE,
    PRIMARY KEY (client_id, scope_id)
);

CREATE TABLE IF NOT EXISTS auth.user_role_mappings (
    user_id INT NOT NULL,
    role_id INT NOT NULL,
    assigned_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_user_roles_user FOREIGN KEY (user_id) REFERENCES auth.users(id) ON DELETE CASCADE,
    CONSTRAINT fk_user_roles_role FOREIGN KEY (role_id) REFERENCES auth.roles(id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

CREATE INDEX idx_user_role_mappings_role_id ON auth.user_role_mappings(role_id);

CREATE TABLE IF NOT EXISTS auth.role_permission_mappings (
    role_id INT NOT NULL,
    permission_id INT NOT NULL,
    reference_id UUID DEFAULT gen_random_uuid() NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_date TIMESTAMPTZ NOT NULL DEFAULT now(),
    updated_date TIMESTAMPTZ NOT NULL DEFAULT now(),

    CONSTRAINT fk_role_permission_mappings_role FOREIGN KEY (role_id) REFERENCES auth.roles(id) ON DELETE CASCADE,
    CONSTRAINT fk_role_permission_mappings_permission FOREIGN KEY (permission_id) REFERENCES auth.permissions(id) ON DELETE CASCADE,
    PRIMARY KEY (role_id, permission_id)
);

CREATE INDEX idx_role_permission_mappings_role_id ON auth.role_permission_mappings(role_id);
