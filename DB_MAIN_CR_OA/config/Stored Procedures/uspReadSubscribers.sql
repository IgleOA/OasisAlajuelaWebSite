-- ======================================================================
-- Name: [config].[uspReadUsersByResource]
-- Desc: Retorna los detalles de usuarios por recurso
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadSubscribers]
	@ResourceID INT = NULL,
	@IsPublic	BIT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@ResourceID IS NOT NULL)
					BEGIN
						SELECT	@IsPublic = RT.[IsPublic]
						FROM	[config].[utbResources] R
								INNER JOIN [config].[utbResourceTypes] RT ON RT.[ResourceTypeID] = R.[ResourceTypeID]		
						WHERE	R.[ResourceID] = @ResourceID
					END
				
				IF(@IsPublic = 1)
					BEGIN
						SELECT	U.[UserID]
								,U.[Email]
						FROM	[adm].[utbUsers] U
						WHERE	U.[ActiveFlag] = 1
								AND U.[Subscriber] = 1
					END
				ELSE
					BEGIN
						SELECT	DISTINCT
								U.[UserID]
								,U.[Email]
						FROM	[config].[utbResources] R
								INNER JOIN [config].[utbResourcesGroups] RG ON RG.[ResourceTypeID] = R.[ResourceTypeID] AND RG.[ActiveFlag] = 1
								INNER JOIN [config].[utbUsersGroups] UG ON UG.[GroupID] = RG.[GroupID] AND UG.[ActiveFlag] = 1
								INNER JOIN [adm].[utbUsers] U ON U.[UserID] = UG.[UserID] AND U.[ActiveFlag] = 1 AND U.[Subscriber] = 1
						WHERE	R.[ResourceID] = @ResourceID
					END
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