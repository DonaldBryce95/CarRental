IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Branches] (
    [BranchId] int NOT NULL IDENTITY,
    [BranchName] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [OpeningHours] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Branches] PRIMARY KEY ([BranchId])
);

CREATE TABLE [Customers] (
    [CustomerId] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerId])
);

CREATE TABLE [VehicleTypes] (
    [VehicleTypeId] int NOT NULL IDENTITY,
    [VehicleCategories] int NOT NULL,
    [PricePerDay] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_VehicleTypes] PRIMARY KEY ([VehicleTypeId])
);

CREATE TABLE [Cars] (
    [CarId] int NOT NULL IDENTITY,
    [BranchId] int NOT NULL,
    [VehicleTypeId] int NOT NULL,
    [NumberOfDoors] int NOT NULL,
    [TransmissionType] int NOT NULL,
    [CarStatus] int NOT NULL,
    [EngineCapacity] decimal(10,2) NOT NULL,
    [HasAC] bit NOT NULL,
    CONSTRAINT [PK_Cars] PRIMARY KEY ([CarId]),
    CONSTRAINT [FK_Cars_Branches_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branches] ([BranchId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Cars_VehicleTypes_VehicleTypeId] FOREIGN KEY ([VehicleTypeId]) REFERENCES [VehicleTypes] ([VehicleTypeId]) ON DELETE CASCADE
);

CREATE TABLE [Reservations] (
    [ReservationId] int NOT NULL IDENTITY,
    [CustomerId] int NOT NULL,
    [CarId] int NOT NULL,
    [PickUpDate] date NOT NULL,
    [ReturnDate] date NOT NULL,
    [PickUpTime] time NOT NULL,
    [ReturnTime] time NOT NULL,
    [PickUpBranchId] int NOT NULL,
    [ReturnBranchId] int NOT NULL,
    [ReservationStatus] int NOT NULL,
    [TotalPrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY ([ReservationId]),
    CONSTRAINT [FK_Reservations_Branches_PickUpBranchId] FOREIGN KEY ([PickUpBranchId]) REFERENCES [Branches] ([BranchId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reservations_Branches_ReturnBranchId] FOREIGN KEY ([ReturnBranchId]) REFERENCES [Branches] ([BranchId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reservations_Cars_CarId] FOREIGN KEY ([CarId]) REFERENCES [Cars] ([CarId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reservations_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([CustomerId]) ON DELETE NO ACTION
);

CREATE TABLE [ReservationCars] (
    [ReservationCarId] int NOT NULL IDENTITY,
    [ReservationId] int NOT NULL,
    [CarId] int NOT NULL,
    CONSTRAINT [PK_ReservationCars] PRIMARY KEY ([ReservationCarId]),
    CONSTRAINT [FK_ReservationCars_Cars_CarId] FOREIGN KEY ([CarId]) REFERENCES [Cars] ([CarId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ReservationCars_Reservations_ReservationId] FOREIGN KEY ([ReservationId]) REFERENCES [Reservations] ([ReservationId]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Cars_BranchId] ON [Cars] ([BranchId]);

CREATE INDEX [IX_Cars_VehicleTypeId] ON [Cars] ([VehicleTypeId]);

CREATE INDEX [IX_ReservationCars_CarId] ON [ReservationCars] ([CarId]);

CREATE INDEX [IX_ReservationCars_ReservationId] ON [ReservationCars] ([ReservationId]);

CREATE INDEX [IX_Reservations_CarId] ON [Reservations] ([CarId]);

CREATE INDEX [IX_Reservations_CustomerId] ON [Reservations] ([CustomerId]);

CREATE INDEX [IX_Reservations_PickUpBranchId] ON [Reservations] ([PickUpBranchId]);

CREATE INDEX [IX_Reservations_ReturnBranchId] ON [Reservations] ([ReturnBranchId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260902180905_InitialCreate', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Cars] ADD [Make] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Cars] ADD [Model] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260902205255_AddCarMakeAndModel', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [CustomerId] int NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUsers_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([CustomerId]) ON DELETE NO ACTION
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [IX_AspNetUsers_CustomerId] ON [AspNetUsers] ([CustomerId]) WHERE [CustomerId] IS NOT NULL;

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260908153818_AddApplicationUser', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [MaintenanceWorkers] (
    [MaintenanceWorkerId] int NOT NULL IDENTITY,
    [MaintenanceWorkerStatus] int NOT NULL,
    CONSTRAINT [PK_MaintenanceWorkers] PRIMARY KEY ([MaintenanceWorkerId])
);

CREATE TABLE [MaintenanceTasks] (
    [MaintenanceTaskId] int NOT NULL IDENTITY,
    [CarId] int NOT NULL,
    [MaintenanceWorkerId] int NOT NULL,
    [MaintenanceTaskStatus] int NOT NULL,
    CONSTRAINT [PK_MaintenanceTasks] PRIMARY KEY ([MaintenanceTaskId]),
    CONSTRAINT [FK_MaintenanceTasks_Cars_CarId] FOREIGN KEY ([CarId]) REFERENCES [Cars] ([CarId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_MaintenanceTasks_MaintenanceWorkers_MaintenanceWorkerId] FOREIGN KEY ([MaintenanceWorkerId]) REFERENCES [MaintenanceWorkers] ([MaintenanceWorkerId]) ON DELETE NO ACTION
);

CREATE TABLE [MaintenanceReports] (
    [MaintenanceReportId] int NOT NULL IDENTITY,
    [MaintenanceTaskId] int NOT NULL,
    [MaintenanceWorkerId] int NOT NULL,
    [InspectionResult] int NOT NULL,
    [MaintenanceReportStatus] int NOT NULL,
    CONSTRAINT [PK_MaintenanceReports] PRIMARY KEY ([MaintenanceReportId]),
    CONSTRAINT [FK_MaintenanceReports_MaintenanceTasks_MaintenanceTaskId] FOREIGN KEY ([MaintenanceTaskId]) REFERENCES [MaintenanceTasks] ([MaintenanceTaskId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_MaintenanceReports_MaintenanceWorkers_MaintenanceWorkerId] FOREIGN KEY ([MaintenanceWorkerId]) REFERENCES [MaintenanceWorkers] ([MaintenanceWorkerId]) ON DELETE NO ACTION
);

CREATE UNIQUE INDEX [IX_MaintenanceReports_MaintenanceTaskId] ON [MaintenanceReports] ([MaintenanceTaskId]);

CREATE INDEX [IX_MaintenanceReports_MaintenanceWorkerId] ON [MaintenanceReports] ([MaintenanceWorkerId]);

CREATE INDEX [IX_MaintenanceTasks_CarId] ON [MaintenanceTasks] ([CarId]);

CREATE INDEX [IX_MaintenanceTasks_MaintenanceWorkerId] ON [MaintenanceTasks] ([MaintenanceWorkerId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260913221612_AddMaintenance', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MaintenanceTasks]') AND [c].[name] = N'MaintenanceWorkerId');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [MaintenanceTasks] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [MaintenanceTasks] ALTER COLUMN [MaintenanceWorkerId] int NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260913223251_MakeMaintenanceWorkerOptional', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [MaintenanceWorkers] ADD [FirstName] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [MaintenanceWorkers] ADD [LastName] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260913234946_NamesToMaintenanceWorker', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [MaintenanceWorkers] ADD [UserId] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260914052612_LinkMaintenanceWorkerToUser', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260915004428_UpdateCarStatus', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MaintenanceReports]') AND [c].[name] = N'MaintenanceWorkerId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [MaintenanceReports] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [MaintenanceReports] ALTER COLUMN [MaintenanceWorkerId] int NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260915053308_MakeWorkerNullableOnReport', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
EXEC sp_rename N'[MaintenanceReports].[InspectionResult]', N'WarningLights', 'COLUMN';

ALTER TABLE [MaintenanceReports] ADD [AirConditioning] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [Battery] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [BodyWork] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [Brakes] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [Engine] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [FuelLevel] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [Glass] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [InteriorCondition] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [Lights] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [OverallResult] int NOT NULL DEFAULT 0;

ALTER TABLE [MaintenanceReports] ADD [Tyres] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260915230258_AddMaintenanceInspectionResults', N'10.0.11');

COMMIT;
GO
