-- ======================================================================
-- Name: [adm].[uspUpdateUserNote]
-- Desc: Se utiliza para responder a una nota de usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspUpdateUserNote]
	@NoteID			INT,
	@InsertUser		VARCHAR(100),
	@UpdateType		VARCHAR(10),
	@ResponseNote	VARCHAR(MAX) = NULL
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
				DECLARE @ToUserID INT
						,@ByUserID INT
						,@RequestNote VARCHAR(MAX)

				IF (@UpdateType = 'READ')
					BEGIN
						UPDATE	[config].[utbUserNotes] 
						SET		[ReadFlag] = 1,
								[LastModifyUser] = @InsertUser,
								[LastModifyDate] = GETDATE()
						WHERE	[NoteID] = @NoteID
					END
				ELSE
					BEGIN
						UPDATE	[config].[utbUserNotes] 
						SET		[ReadFlag] = 1,
								[ResponseRequired] = 0,
								[ResponseNote] = @ResponseNote,
								[ResponseDate] = GETDATE(),
								[LastModifyUser] = @InsertUser,
								[LastModifyDate] = GETDATE()
						WHERE	[NoteID] = @NoteID

						SELECT	@ToUserID = [InsertUserID],
								@ByUserID = [UserID],
								@RequestNote = [RequestNote]
						FROM	[config].[utbUserNotes]
						WHERE	[NoteID] = @NoteID

						INSERT INTO [config].[utbUserNotes] ([UserID],[RequestNote],[ResponseRequired],[ResponseNote],[ResponseDate],[InsertUserID],[LastModifyUser])
						VALUES (@ToUserID, @RequestNote, 0, @ResponseNote, GETDATE(), @ByUserID, @InsertUser)						
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