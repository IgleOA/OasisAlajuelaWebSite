-- ======================================================================
-- Name: [adm].[uspUpdateWebDirectory]
-- Desc: Se utiliza para actualizar la información un directorio web
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 01/06/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateWebDirectory]
	@InsertUser		VARCHAR(50),
    @ActionType     VARCHAR(10),
	@WebID          INT,
    @AppID			INT = NULL,    
	@Controller		VARCHAR(50) = NULL,
	@Action			VARCHAR(50) = NULL,
	@PublicMenu		BIT = NULL,
	@AdminMenu		BIT = NULL,
	@DisplayName	VARCHAR(50) = NULL,
	@Order			INT = NULL,
    @ActiveFlag     BIT = NULL,
	@Parameter		VARCHAR(50) = NULL	
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
                IF (@ActionType = 'CHGST')
                    BEGIN
                        DECLARE @Status BIT
                        SELECT  @Status = [ActiveFlag]
                        FROM    [adm].[utbWebDirectory] 
                        WHERE   [WebID] = @WebID
                        
                        IF (@Status = 1)
                            BEGIN
                                UPDATE  [adm].[utbWebDirectory] 
                                SET     [ActiveFlag] = 0
                                        ,[LastModifyDate] = GETDATE()
                                        ,[LastModifyUser] = @InsertUser
				                WHERE   [WebID] = @WebID
                            END
                        ELSE
                            BEGIN
                                UPDATE  [adm].[utbWebDirectory] 
                                SET     [ActiveFlag] = 1
                                        ,[LastModifyDate] = GETDATE()
                                        ,[LastModifyUser] = @InsertUser
				                WHERE   [WebID] = @WebID
                            END
                    END
                ELSE 
                    BEGIN
				        UPDATE  [adm].[utbWebDirectory] 
                        SET     [AppID] = @AppID
                                ,[Controller] = @Controller
                                ,[Action] = ISNULL(@Action,'Index')
                                ,[PublicMenu] = @PublicMenu
                                ,[AdminMenu] = @AdminMenu
                                ,[DisplayName] = @DisplayName
                                ,[Parameter] = @Parameter
                                ,[Order] = @Order
                                ,[ActiveFlag] = @ActiveFlag
                                ,[LastModifyDate] = GETDATE()
                                ,[LastModifyUser] = @InsertUser
				        WHERE   [WebID] = @WebID
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
GO