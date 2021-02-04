-- ======================================================================
-- Name: [config].[uspReadContacts]
-- Desc: Retorna las solicitudes de Contacto
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 02/04/2021
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadContacts]
	@ContactID		INT = NULL,
	@HistoryFlag	BIT = NULL,
	@ContactTypeID	INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@ContactID IS NOT NULL)
					BEGIN
						SELECT	[ContactID]
								,[Requester]
								,[Email]
								,[PhoneNumber]
								,[InsertDate]	= DATEADD(HOUR,-6,[InsertDate])
								,[ContactTypeID]
								,[Reason]
								,[IP]
								,[Country]
								,[Region]
								,[City]
						FROM	[config].[utbContacts] 
						WHERE	[ContactID] = @ContactID
					END
				ELSE
					BEGIN
						IF(@HistoryFlag = 1)
							BEGIN
								SELECT	[ContactID]
										,[Requester]
										,[Email]
										,[PhoneNumber]
										,[InsertDate]	= DATEADD(HOUR,-6,[InsertDate])
										,[ContactTypeID]
										,[Reason]
										,[IP]
										,[Country]
										,[Region]
										,[City]
								FROM	[config].[utbContacts] 
								WHERE	[ContactTypeID] = ISNULL(@ContactTypeID,[ContactTypeID])
								ORDER BY [InsertDate] DESC
							END
						ELSE 
							BEGIN
								SELECT	[ContactID]
										,[Requester]
										,[Email]
										,[PhoneNumber]
										,[InsertDate]	= DATEADD(HOUR,-6,[InsertDate])
										,[ContactTypeID]
										,[Reason]
										,[IP]
										,[Country]
										,[Region]
										,[City]
								FROM	[config].[utbContacts] 
								WHERE	[ContactTypeID] = ISNULL(@ContactTypeID,[ContactTypeID])
										AND DATEDIFF(MONTH,DATEADD(HOUR,-6,[InsertDate]),GETDATE()) = 0
								ORDER BY [InsertDate] DESC
							END
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


