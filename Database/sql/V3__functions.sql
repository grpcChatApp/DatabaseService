CREATE OR REPLACE FUNCTION auth.get_client_scopes(p_client_id TEXT)
RETURNS TABLE(scope_name TEXT) AS $$
BEGIN
    RETURN QUERY
    SELECT s.name
    FROM auth.clients c
    JOIN auth.client_resource_mappings crm ON crm.client_id = c.id
    JOIN auth.resource_scope_mappings rsm ON rsm.resource_id = crm.resource_id
    JOIN auth.scopes s ON s.id = rsm.scope_id
    WHERE c.client_id = p_client_id
      AND c.is_active = TRUE
      AND s.is_active = TRUE;
END;
$$ LANGUAGE plpgsql;
