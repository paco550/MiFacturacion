MiFacturación
MiFacturación es una aplicación de facturación para pequeñas empresas y autónomos, diseñada para simplificar la gestión de ingresos y gastos. Este sistema permite registrar y organizar las facturas de forma fácil y eficiente, con un enfoque en la usabilidad y la escalabilidad.

Características
Gestión de Facturas: Creación, edición y eliminación de facturas.
Control de Ingresos y Gastos: Seguimiento detallado de la actividad financiera.
Organización de Clientes y Proveedores: Sistema para almacenar información relevante.
Exportación de Datos: Opciones para exportar registros en distintos formatos (ej. PDF).
Tecnologías
Backend: ASP.NET Core
Frontend: Angular
Base de Datos: SQL Server
Instalación
Clona el repositorio:
bash
Copiar código
git clone https://github.com/paco550/MiFacturacion.git
Configura la base de datos en el archivo de configuración.
Ejecuta los servicios del backend y frontend:
Backend: dotnet run
Frontend: ng serve
Despliegue
Backend
Configura la base de datos en un entorno de producción (ej., Azure SQL o SQL Server en un servidor propio).
Publica el proyecto en el servidor:
bash
Copiar código
dotnet publish -c Release -o ./publish
Configura el servidor web (ej. IIS o NGINX) para apuntar al directorio de publicación.
Establece las variables de entorno necesarias para producción (ej., cadena de conexión de la base de datos).
Frontend
Genera la build de producción de Angular:
bash
Copiar código
ng build --prod
Configura un servidor web para servir la aplicación (ej. Apache o NGINX) en el directorio de build generado (/dist).
Asegura que el frontend esté configurado para comunicarse con el backend en el entorno de producción.
