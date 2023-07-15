CREATE TABLE Usuarios (
  id_usuario INT PRIMARY KEY,
  nombre_usuario VARCHAR(100),
  contrasena VARCHAR(100),
  perfil VARCHAR(50)
);

CREATE TABLE Sucursales (
  id_sucursal INT PRIMARY KEY,
  nombre_sucursal VARCHAR(100)
);

CREATE TABLE Colaboradores (
  id_colaborador INT PRIMARY KEY,
  nombre_colaborador VARCHAR(100)
);

INSERT INTO Usuarios (id_usuario, nombre_usuario, contrasena, perfil)
VALUES (1, 'Juana Perez', 'contrasena', 'Gerente de tienda');

INSERT INTO Usuarios (id_usuario, nombre_usuario, contrasena, perfil)
VALUES (2, 'Pedro Sanchez', 'contrasena2', 'Jefe de Tienda');

INSERT INTO Colaboradores (id_colaborador, nombre_colaborador)
VALUES (1, 'Maria Hernandez');

INSERT INTO Colaboradores (id_colaborador, nombre_colaborador)
VALUES (2, 'Jose Feliciano');

INSERT INTO Colaboradores (id_colaborador, nombre_colaborador)
VALUES (3, 'Juan Alvarez');

INSERT INTO Colaboradores (id_colaborador, nombre_colaborador)
VALUES (4, 'Andrea Gutierrez');

INSERT INTO Colaboradores (id_colaborador, nombre_colaborador)
VALUES (5, 'Mauricio Sabillon');

INSERT INTO Colaboradores (id_colaborador, nombre_colaborador)
VALUES (6, 'Federico Valverde');

INSERT INTO Colaboradores (id_colaborador, nombre_colaborador)
VALUES (7, 'Leo Messi');

INSERT INTO Colaboradores (id_colaborador, nombre_colaborador)
VALUES (8, 'Jose Fernandez');

INSERT INTO Sucursales (id_sucursal, nombre_sucursal)
VALUES (1, 'FARSIMAN CHOLOMA');

INSERT INTO Sucursales (id_sucursal, nombre_sucursal)
VALUES (2, 'FARSIMAN JUAN PABLO');

select * from Colaboradores






CREATE TABLE Asignaciones (
  id_asignacion INT IDENTITY(1,1) PRIMARY KEY,
  id_colaborador INT,
  id_sucursal INT,
  distancia_km DECIMAL(10, 2),
  FOREIGN KEY (id_colaborador) REFERENCES Colaboradores(id_colaborador),
  FOREIGN KEY (id_sucursal) REFERENCES Sucursales(id_sucursal),
  CONSTRAINT uc_asignacion UNIQUE (id_colaborador, id_sucursal),
  CONSTRAINT chk_distancia_km CHECK (distancia_km > 0 AND distancia_km <= 50)
);


CREATE TABLE Transportistas (
  id_transportista INT PRIMARY KEY,
  nombre_transportista VARCHAR(100),
  tarifa_km DECIMAL(10, 2)
);

INSERT INTO Transportistas(id_transportista, nombre_transportista, tarifa_km)
VALUES (1, 'Juanito Perez', 50);

INSERT INTO Transportistas(id_transportista, nombre_transportista, tarifa_km)
VALUES (2, 'Fulanito Pangano', 45);



select * from Viajes
select * from Asignaciones

CREATE TABLE Viajes (
  id_viaje INT IDENTITY(1,1) PRIMARY KEY,
  id_colaborador INT,
  id_sucursal INT,
  id_transportista INT,
  distancia_km DECIMAL(10, 2),
  fecha DATE,
  usuario_registro VARCHAR(50),
  FOREIGN KEY (id_colaborador) REFERENCES Colaboradores(id_colaborador),
  FOREIGN KEY (id_sucursal) REFERENCES Sucursales(id_sucursal),
  FOREIGN KEY (id_transportista) REFERENCES Transportistas(id_transportista),
  CONSTRAINT chk_distancia2_km CHECK (distancia_km > 0 AND distancia_km <= 100),
  CONSTRAINT uc_colaborador_fecha UNIQUE (id_colaborador, fecha)
);

