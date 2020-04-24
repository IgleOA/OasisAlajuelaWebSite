-- ======================================================================
-- Name: [config].[uspReadResourceTypes]
-- Desc: Retorna los detalles de los tipos de recursos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadResourceTypes]
	@UserName VARCHAR(100)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				DECLARE @WriteFlag	BIT
						,@PasFlag	BIT
				DECLARE @Data	TABLE ( [ResourceTypeID] INT
										,[TypeName] VARCHAR(100)
										,[Description] VARCHAR(MAX)
										,[TypeImage] VARBINARY(MAX)
										,[TypeImageExt] VARCHAR(10)
										,[IsPublic]	BIT
										,[ActiveFlag] BIT )

				INSERT INTO @Data
				SELECT	[ResourceTypeID]
						,[TypeName]
						,[Description]
						,[TypeImage]
						,[TypeImageExt]
						,[IsPublic]
						,[ActiveFlag]  
				FROM	[config].[utbResourceTypes]
				WHERE	[IsPublic] = 1
						AND [ActiveFlag] = 1

				SELECT	@WriteFlag = ISNULL(RR.[Write],0)
				FROM	[adm].[utbUsers] U
						OUTER APPLY (SELECT R.[Write]
									 FROM	[adm].[utbRightsbyRole] R 
											LEFT JOIN [adm].[utbWebDirectory] WD ON WD.[WebID] = R.[WebID]
									 WHERE	U.[RoleID] = R.[RoleID]
											AND R.[ActiveFlag] = 1
											AND WD.[Controller] = 'Resources') RR
				WHERE	U.[UserName] = @UserName		

				SELECT	@PasFlag = ISNULL(R.[ActiveFlag],0)
				FROM	[adm].[utbUsers] U
						OUTER APPLY (SELECT UG.[ActiveFlag]
									 FROM	[config].[utbUsersGroups] UG 
											LEFT JOIN [config].[utbGroups] G ON G.[GroupID] = UG.[GroupID] 
									 WHERE	U.[UserID] = UG.[UserID]
											AND UG.[ActiveFlag] = 1
											AND G.[ActiveFlag] = 1
											AND G.[GroupName] = 'Cuerpo Pastoral') R
				WHERE	U.[UserName] = @UserName

				IF (@WriteFlag = 1 OR @PasFlag = 1)
					BEGIN
						INSERT INTO @Data
						SELECT	[ResourceTypeID]
								,[TypeName]
								,[Description]
								,[TypeImage]
								,[TypeImageExt]
								,[IsPublic]
								,[ActiveFlag]     
						FROM	[config].[utbResourceTypes]
						WHERE	[IsPublic] = 0
								AND [ActiveFlag] = 1
					END
				ELSE
					BEGIN
						INSERT INTO @Data
						SELECT	RT.[ResourceTypeID]
								,RT.[TypeName]
								,RT.[Description]
								,RT.[TypeImage]
								,RT.[TypeImageExt]
								,RT.[IsPublic]
								,RT.[ActiveFlag]      
						FROM	[config].[utbResourceTypes] RT
								INNER JOIN [config].[utbResourcesGroups] RG ON RG.[ResourceTypeID] = RT.[ResourceTypeID] AND RG.[ActiveFlag] = 1
								INNER JOIN [config].[utbUsersGroups] UG ON UG.[GroupID] = RG.[GroupID] AND UG.[ActiveFlag] = 1
								INNER JOIN [adm].[utbUsers] U ON U.[UserID] = UG.[UserID] AND U.[UserName] = @UserName
						WHERE	RT.[IsPublic] = 0
								AND RT.[ActiveFlag] = 1
					END

				SELECT	DISTINCT
						[ResourceTypeID]
						,[TypeName]
						,[Description]
						,[TypeImage]
						,[TypeImageExt]
						,[IsPublic]
						,[ActiveFlag]   
				FROM	@Data
			-- =======================================================

        END TRY
        BEGIN CATCH

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END
    SET NOCOUNT OFF
