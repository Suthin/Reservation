USE [Reservation]
GO
SET IDENTITY_INSERT [dbo].[Client] ON 
GO
INSERT [dbo].[Client] ([ClientId], [ClientName], [Description], [Status], [CreatedOn], [LastUpdatedOn]) VALUES (1, N'Client 1', NULL, 1, CAST(N'2023-01-03T00:00:00.0000000' AS DateTime2), CAST(N'2023-01-03T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Client] OFF
GO
SET IDENTITY_INSERT [dbo].[Provider] ON 
GO
INSERT [dbo].[Provider] ([ProviderId], [ProviderName], [Description], [Status], [CreatedOn], [LastUpdatedOn]) VALUES (1, N'Provider 1', NULL, 1, CAST(N'2023-01-03T00:00:00.0000000' AS DateTime2), CAST(N'2023-01-03T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Provider] OFF
GO
