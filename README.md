 
    
INFORME DE GUÍA PRÁCTICA  
   
I. PORTADA  
  
Tema:                                                      Desarrolle el aplicativo Banco con el consumo de datos 
                                                                 y metodos CRUD ( Interfaz de usuario Swagger), debe   
                                                                 enviar.
Unidad de Organización Curricular:  Básica  
Nivel y Paralelo:                                    3ro TI   
Alumnos participantes:                        Yaguachi Coba Andrew Sebastian 
Asignatura:                                            Programación Avanzada  
Docente:                                                  Ing. Caiza Caizabuano José Rubén  
  
 1. Introducción
El presente informe describe el desarrollo de un sistema informático bancario que permite realizar operaciones básicas sobre cuentas de clientes mediante una arquitectura cliente-servidor. El sistema incluye una API RESTful construida en .NET Core con documentación Swagger, un cliente de escritorio en Windows Forms, y una base de datos relacional (SQL Server). Se implementan operaciones CRUD y transacciones seguras para garantizar la integridad de los datos.

2. Objetivos
2.1 Objetivo general
Desarrollar un sistema bancario funcional con interfaz de usuario y backend API RESTful que permita realizar operaciones de mantenimiento y transacciones seguras entre cuentas.
2.2 Objetivos específicos
•	Crear una base de datos relacional para gestionar cuentas, clientes y transacciones.
•	Implementar un backend con .NET Core que exponga endpoints RESTful documentados con Swagger.
•	Crear una interfaz gráfica de usuario en Windows Forms para consumo de la API.
•	Implementar mecanismos de manejo de errores y control de transacciones ACID.

3. Arquitectura del sistema
El sistema está dividido en tres capas principales:
1.	Capa de presentación: aplicación de escritorio en Windows Forms.
2.	Capa de lógica de negocio: API RESTful en .NET Core 6.
3.	Capa de datos: base de datos SQL Server.

4. Herramientas utilizadas
•	Visual Studio 2022
•	SQL Server Management Studio
•	.NET 6.0
•	Windows Forms
•	Entity Framework Core
•	Swagger (Swashbuckle)
•	GitHub para control de versiones

5. Base de datos
La base de datos se llama BancoDB y contiene las siguientes tablas principales:
•	Clientes (Id, Nombre, Identificacion, Dirección)
•	Cuentas (Id, ClienteId, NumeroCuenta, Saldo)
•	Transacciones (Id, CuentaOrigenId, CuentaDestinoId, Monto, Fecha).

Script de creación de base de datos:

 


6. Backend (.NET API + Swagger)
Funcionalidades implementadas
•	CRUD de clientes y cuentas
•	Transferencia entre cuentas
•	Registro de transacciones
•	Validaciones de saldo
•	Manejo de excepciones





Ejemplo de endpoint:

 ![image](https://github.com/user-attachments/assets/c4a0a8bf-a9a8-4c2b-9086-0b0502de619b)



Documentación Swagger
La documentación de los endpoints fue generada automáticamente mediante Swashbuckle y permite probar el sistema directamente desde el navegador.

7. Frontend (Windows Forms)
La aplicación de escritorio se conecta a la API utilizando HttpClient, permitiendo realizar las siguientes acciones:
•	Registrar clientes
•	Crear cuentas bancarias
•	Consultar cuentas
•	Ejecutar transferencias entre cuentas
Ejemplo de consumo de API:

 
![image](https://github.com/user-attachments/assets/1ac617b1-8fa9-48f4-abfa-ac4c0c01a6ae)




8. Transacciones y manejo de errores
Se utilizó Database.BeginTransactionAsync() para garantizar la atomicidad en operaciones críticas como transferencias.
En caso de error (por ejemplo, saldo insuficiente o ID inválido), la transacción se revierte automáticamente mediante RollbackAsync().
Además, se implementaron respuestas HTTP apropiadas:
•	200 OK para éxito
•	400 BadRequest para validaciones
•	500 InternalServerError para errores del servidor

9. Repositorio GitHub

https://github.com/TheBearForce/BancoAPI 

 

10. Conclusión
El desarrollo del sistema bancario ha permitido aplicar principios de diseño en capas, servicios web RESTful, consumo de APIs desde aplicaciones de escritorio, y el uso de transacciones para garantizar la integridad de los datos. El uso de herramientas modernas como Swagger ha facilitado la validación y prueba de los servicios desarrollados.

