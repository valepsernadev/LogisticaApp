-- =============================================
-- LogisticaApp - Script de inicialización de BD
-- PostgreSQL 16
-- =============================================

-- 1. Usuarios
CREATE TABLE usuarios (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nombre VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    rol VARCHAR(20) NOT NULL CHECK (rol IN ('cliente', 'conductor', 'operador', 'administrador')),
    activo BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 2. Conductores
CREATE TABLE conductores (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id UUID NOT NULL UNIQUE REFERENCES usuarios(id),
    licencia VARCHAR(50) NOT NULL,
    activo BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 3. Vehículos
CREATE TABLE vehiculos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    placa VARCHAR(20) NOT NULL UNIQUE,
    capacidad_kg DECIMAL(10,2) NOT NULL,
    conductor_id UUID REFERENCES conductores(id),
    activo BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 4. Envíos
CREATE TABLE envios (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cliente_id UUID NOT NULL REFERENCES usuarios(id),
    conductor_id UUID REFERENCES conductores(id),
    vehiculo_id UUID REFERENCES vehiculos(id),
    estado VARCHAR(20) NOT NULL DEFAULT 'pendiente' CHECK (estado IN ('pendiente', 'asignado', 'en_transito', 'entregado', 'cancelado')),
    origen VARCHAR(255) NOT NULL,
    destino VARCHAR(255) NOT NULL,
    tipo_mercancia VARCHAR(100),
    peso_kg DECIMAL(10,2),
    observaciones TEXT,
    fecha_estimada_entrega TIMESTAMP,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 5. Rutas
CREATE TABLE rutas (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    envio_id UUID NOT NULL REFERENCES envios(id),
    distancia_km DECIMAL(10,2),
    tiempo_estimado_minutos INTEGER,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 6. Evidencias
CREATE TABLE evidencias (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    envio_id UUID NOT NULL REFERENCES envios(id),
    conductor_id UUID NOT NULL REFERENCES conductores(id),
    foto_url VARCHAR(500) NOT NULL,
    latitud DECIMAL(10,7),
    longitud DECIMAL(10,7),
    fecha_hora TIMESTAMP NOT NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 7. Incidencias
CREATE TABLE incidencias (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    envio_id UUID NOT NULL REFERENCES envios(id),
    conductor_id UUID NOT NULL REFERENCES conductores(id),
    tipo VARCHAR(50) NOT NULL,
    descripcion TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 8. Notificaciones
CREATE TABLE notificaciones (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id UUID NOT NULL REFERENCES usuarios(id),
    titulo VARCHAR(150) NOT NULL,
    mensaje TEXT NOT NULL,
    leida BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 9. Tarifas
CREATE TABLE tarifas (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    tipo VARCHAR(50) NOT NULL,
    valor DECIMAL(10,2) NOT NULL,
    descripcion TEXT,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 10. Favoritos del cliente
-- El campo "tipo" puede ser: "direccion" o "destinatario"
-- El campo "valor" guarda la dirección completa si tipo='direccion'
-- o el email/teléfono del destinatario si tipo='destinatario'
CREATE TABLE favoritos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    cliente_id UUID NOT NULL REFERENCES usuarios(id),
    tipo VARCHAR(20) NOT NULL CHECK (tipo IN ('direccion', 'destinatario')),
    nombre VARCHAR(100) NOT NULL,
    valor VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW()
);
