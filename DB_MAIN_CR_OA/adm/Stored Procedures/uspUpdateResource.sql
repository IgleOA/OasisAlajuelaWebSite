-- ======================================================================
-- Name: [adm].[uspUpdateResource]
-- Desc: Permite actualizar la informacion de un recurso.
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================
CREATE PROCEDURE [adm].[uspUpdateResource]
	@InsertUser		VARCHAR(50),	
	@ResourceID		INT,
	@ActionType		VARCHAR(10),
	@ResourceTypeID	INT = NULL,
	@FileName		VARCHAR(500)	= NULL,
	@Description	VARCHAR(MAX)	= NULL	
AS 
    BEGIN
        SET NOCOUNT ON
        SET XACT_ABORT ON
                           
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
            DECLARE @lLocalTran BIT = 0
                               
            IF @@TRANCOUNT = 0 
                BEGIN
                    BEGIN TRANSACTION
                    SET @lLocalTran = 1
                END

            -- =======================================================
				IF(@ActionType = 'CHGST')
					BEGIN
						UPDATE	[config].[utbResources]
						SET		[ActiveFlag] = 0
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[ResourceID] = @ResourceID
						
					END
				ELSE	
					BEGIN
						UPDATE	[config].[utbResources]
						SET		[ResourceTypeID] = @ResourceTypeID
								,[FileName] = @FileName
								,[Description] = @Description
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[ResourceID] = @ResourceID
					END
			-- =======================================================

        IF ( @@trancount > 0
                 AND @lLocalTran = 1
               ) 
                BEGIN
                    COMMIT TRANSACTION
                END
        END TRY
        BEGIN CATCH
            IF ( @@trancount > 0
                 AND XACT_STATE() <> 0
               ) 
                BEGIN
                    ROLLBACK TRANSACTION
                END

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END

    SET NOCOUNT OFF
    SET XACT_ABORT OFF
