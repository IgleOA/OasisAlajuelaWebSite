-- ======================================================================
-- Name: [book].[uspAddReservation]
-- Desc: Se utiliza para la creación de una nueva reserva
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================
CREATE PROCEDURE [book].[uspAddReservation]
	@GUID		    VARCHAR(MAX),
	@EventID	    INT,
	@BookedBy	    INT,
	@JSONBookedFor	NVARCHAR(MAX),
	@InsertUser	    VARCHAR(100)
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
                DECLARE @pGUID			VARCHAR(MAX),
						@pEventID		INT,
						@pBookedBy		INT,
						@pFirstName		VARCHAR(100),
						@pLastName		VARCHAR(100),
						@pIdentityID	VARCHAR(100),
						@pInsertUser	VARCHAR(100)
						   
				SELECT	[GUID]		= @GUID
						,[EventID]	= @EventID
						,[BookedBy]	= @BookedBy
						,[FirstName]	= TRIM([FirstName])
						,[LastName]		= TRIM([LastName])
						,[IdentityID]
						,[InsertUser]	= @InsertUser
						,[IsValid]	= 0
				INTO	#CursorData
				FROM	OPENJSON ( @JSONBookedFor )  
				WITH	(
							FirstName	varchar(100) '$.FirstName' ,  
							LastName	varchar(100) '$.LastName' ,  
							IdentityID	varchar(100) '$.IdentityID' 
						)

				DECLARE ActionProcess CURSOR FOR 
				SELECT	[GUID]
						,[EventID]
						,[BookedBy]
						,[FirstName] 
						,[LastName]
						,[IdentityID]
						,[InsertUser] 
				FROM	#CursorData

				OPEN ActionProcess
				FETCH NEXT FROM ActionProcess INTO @pGUID, @pEventID, @pBookedBy, @pFirstName, @pLastName, @pIdentityID, @pInsertUser

				WHILE @@FETCH_STATUS = 0
					BEGIN
						IF NOT EXISTS ( SELECT *
										FROM	[book].[utbReservations] 
										WHERE	[EventID] = @pEventID
												AND [IdentityID] = @pIdentityID
												AND [ActiveFlag] = 1)
							BEGIN
								INSERT INTO [book].[utbReservations] ([GUID],[EventID],[BookedBy],[FirstName], [LastName],[IdentityID],[InsertUser],[LastModifyUser])
								VALUES (@pGUID, @pEventID, @pBookedBy, @pFirstName, @pLastName, @pIdentityID,@pInsertUser, @pInsertUser)

								UPDATE	#CursorData
								SET		[IsValid] = 1
								WHERE	[IdentityID] = @pIdentityID
							END						
						FETCH NEXT FROM ActionProcess INTO @pGUID, @pEventID, @pBookedBy, @pFirstName, @pLastName, @pIdentityID, @pInsertUser
					END

					CLOSE ActionProcess
					DEALLOCATE ActionProcess

					SELECT * FROM #CursorData
	
					DROP TABLE #CursorData
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