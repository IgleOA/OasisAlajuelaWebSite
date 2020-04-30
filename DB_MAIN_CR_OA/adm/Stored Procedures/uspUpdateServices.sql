-- ======================================================================
-- Name: [adm].[uspUpdateServices]
-- Desc: Se utiliza para actualizar la informacion de los servicios
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateServices]
	@User			VARCHAR(50),
	@ServiceID		INT,
	@SVCIcon		VARCHAR(50) = NULL,
	@SVCName		VARCHAR(50) = NULL,
	@SVCDescription	VARCHAR(MAX) = NULL,
	@SVCOrder		INT = NULL,
	@ActiveFlag		BIT = NULL
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
				IF (@SVCName IS NULL)
					BEGIN
						DECLARE @Status BIT
						SELECT	@Status = [ActiveFlag]
						FROM	[config].[utbServices]
						WHERE	[ServiceID] = @ServiceID

						IF(@Status = 1)
							BEGIN
								UPDATE	[config].[utbServices] 
								SET		[ActiveFlag] = 0
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @User
								WHERE	[ServiceID] = @ServiceID
							END
						ELSE
							BEGIN
								UPDATE	[config].[utbServices] 
								SET		[ActiveFlag] = 1
										,[LastModifyDate] = GETDATE()
										,[LastModifyUser] = @User
								WHERE	[ServiceID] = @ServiceID
							END
					END
				ELSE
					BEGIN
						UPDATE	[config].[utbServices] 
						SET		 [ServiceName]		= @SVCName
								,[ServiceDescription]	= @SVCDescription	
								,[ServiceIcon]		= @SVCIcon
								,[Order]			= @SVCOrder
								,[ActiveFlag]		= @ActiveFlag
								,[LastModifyDate]	= GETDATE()
								,[LastModifyUser]	= @User
						WHERE	[ServiceID] = @ServiceID
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