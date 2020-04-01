-- ======================================================================
-- Name: [adm].[uspUpdateRights]
-- Desc: Se utiliza para agregar o actualizar un derecho
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/28/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateRights]
	@InsertUser	VARCHAR(50),
	@WebID		INT,
	@RoleID		INT,
	@RightID	INT,
	@Read		BIT,
	@Write		BIT
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
				IF(@RightID > 0)
					BEGIN
						UPDATE	[adm].[utbRightsbyRole]
						SET		[Read]		= @Read	
								,[Write] 	= @Write
								,[LastModifyUser]	= @InsertUser
								,[LastModifyDate]	= GETDATE()
						WHERE	[RightID] = @RightID
					END
				ELSE
					BEGIN
						INSERT INTO [adm].[utbRightsbyRole] ([WebID],[RoleID],[Read],[Write],[CreationUser],[LastModifyUser])
						VALUES (@WebID, @RoleID, @Read, @Write, @InsertUser, @InsertUser)
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
