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
	@GUID		VARCHAR(MAX),
	@EventID	INT,
	@Seatlist	VARCHAR(MAX),
	@BookedBy	INT,
	@BookedFor	VARCHAR(100),
	@InsertUser	VARCHAR(100)
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
						@pSeatID		VARCHAR(10),
						@pBookedBy		INT,
						@pBookedFor		VARCHAR(100),
						@pInsertUser	VARCHAR(100)

				SELECT	[SeatID] = [Item]
						,[IsValid] = 0
				INTO	#Results
				FROM	[SplitValues] (@Seatlist)
			
				SELECT	[GUID]			= @GUID
						,[EventID]		= @EventID
						,[SeatID]		
						,[BookedBy]		= @BookedBy
						,[BookedFor]	= @BookedFor
						,[InsertUser]   = @InsertUser
				INTO	#CursorData
				FROM	#Results

				DECLARE ActionProcess CURSOR FOR
				SELECT	[GUID]
						,[EventID]
						,[SeatID]		
						,[BookedBy]
						,[BookedFor]
						,[InsertUser]
				FROM	#CursorData

				OPEN ActionProcess
				FETCH NEXT FROM ActionProcess INTO @pGUID, @pEventID, @pSeatID, @pBookedBy, @pBookedFor,@pInsertUser

				WHILE @@FETCH_STATUS = 0
					BEGIN
						IF NOT EXISTS ( SELECT *
										FROM	[book].[utbReservations] 
										WHERE	[EventID] = @pEventID
												AND [SeatID] = @pSeatID
												AND [ActiveFlag] = 1)
							BEGIN
								INSERT INTO [book].[utbReservations] ([GUID],[EventID],[SeatID],[BookedBy],[BookedFor],[InsertUser],[LastModifyUser])
								VALUES (@pGUID, @pEventID, @pSeatID, @pBookedBy, @pBookedFor,@pInsertUser, @pInsertUser)

								UPDATE	#Results
								SET		[IsValid] = 1
								WHERE	[SeatID] = @pSeatID
							END
						FETCH NEXT FROM ActionProcess INTO @pGUID, @pEventID, @pSeatID, @pBookedBy, @pBookedFor,@pInsertUser
					END

					CLOSE ActionProcess
					DEALLOCATE ActionProcess

					SELECT * FROM #Results

					DROP TABLE #Results
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