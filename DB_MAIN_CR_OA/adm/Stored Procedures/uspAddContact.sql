-- ======================================================================
-- Name: [adm].[uspAddContact]
-- Desc: Se utiliza para la insertar una nueva solicitud de Contact
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 5/24/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddContact]
	@Requester		VARCHAR(100),
	@Email			VARCHAR(100),
	@PhoneNumber	VARCHAR(50) = NULL,
    @ContactType    VARCHAR(100),
	@Reason			VARCHAR(1000),
	@IP				VARCHAR(20) = NULL,
	@Country		VARCHAR(50) = NULL,
	@Region			VARCHAR(50) = NULL,
	@City			VARCHAR(50) = NULL
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
				INSERT INTO [config].[utbContacts] ([Requester],[Email],[PhoneNumber],[ContactType],[Reason],[IP],[Country],[Region],[City])
				VALUES (@Requester, @Email, @PhoneNumber, @ContactType, @Reason, @IP, @Country, @Region, @City)
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