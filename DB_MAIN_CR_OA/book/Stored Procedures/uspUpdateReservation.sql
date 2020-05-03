-- ======================================================================
-- Name: [book].[uspUpdateReservation]
-- Desc: Se utiliza para actualizar una reserva
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/27/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================
CREATE PROCEDURE [book].[uspUpdateReservation]
	@InsertUser		VARCHAR(100),
	@ReservationID	INT = NULL,
	@GUID			VARCHAR(MAX) = NULL
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
				IF(@GUID IS NOT NULL)
					BEGIN
						UPDATE	[book].[utbReservations] 
						SET		[ActiveFlag] = 0
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[GUID] = @GUID
					END
				ELSE
					BEGIN
						UPDATE	[book].[utbReservations] 
						SET		[ActiveFlag] = 0
								,[LastModifyDate] = GETDATE()
								,[LastModifyUser] = @InsertUser
						WHERE	[ReservationID] = @ReservationID

						SELECT	[GUID]
						FROM	[book].[utbReservations] 
						WHERE	[ReservationID] = @ReservationID
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