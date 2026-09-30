
using CarStore.Shared.C_DTOs.PurchaseModuleDtos.QueriesDTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CarStore.Services
{
    public class PurchaseConfirmationPdfService
    {
        public byte[] GenerateConfirmationPdf(
            PurchaseConfirmationDTO confirmation)
        {
            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Header
                    page.Header()
                        .Background(Colors.Blue.Darken2)
                        .Padding(20)
                        .Column(column =>
                        {
                            column.Item()
                                .Text("CAR'S WROOM")
                                .FontSize(26)
                                .Bold()
                                .FontColor(Colors.White);

                            column.Item()
                                .Text("PURCHASE CONFIRMATION")
                                .FontSize(16)
                                .FontColor(Colors.White);
                        });

                    // Content
                    page.Content()
                        .PaddingVertical(20)
                        .Column(column =>
                        {
                            column.Spacing(15);

                            // Confirmation Information
                            column.Item()
                                .Text($"Confirmation No: {confirmation.ReferenceNumber}")
                                .FontSize(16)
                                .Bold();

                            column.Item()
                                .Text($"Purchase Method: {confirmation.PurchaseMethod}");

                            // Vehicle Details
                            column.Item()
                                .Text("VEHICLE DETAILS")
                                .FontSize(18)
                                .Bold()
                                .FontColor(Colors.Blue.Darken2);

                            column.Item()
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(15)
                                .Column(vehicle =>
                                {
                                    vehicle.Spacing(5);

                                    vehicle.Item()
                                        .Text(confirmation.SelectedVehicle.Name)
                                        .FontSize(16)
                                        .Bold();

                                    vehicle.Item()
                                        .Text($"Year: {confirmation.SelectedVehicle.Year}");

                                    vehicle.Item()
                                        .Text($"Sale Price: {confirmation.SelectedVehicle.SalePrice:N2}");
                                });

                            // Payment Details
                            column.Item()
                                .Text("PAYMENT DETAILS")
                                .FontSize(18)
                                .Bold()
                                .FontColor(Colors.Blue.Darken2);

                            column.Item()
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(15)
                                .Column(payment =>
                                {
                                    payment.Spacing(5);

                                    payment.Item()
                                        .Text($"Total Price: {confirmation.TotalPrice:N2}");

                                    if (confirmation.FinanceDetails != null)
                                    {
                                        payment.Item()
                                            .Text($"Down Payment: {confirmation.FinanceDetails.DownPayment:N2}");

                                        payment.Item()
                                            .Text($"Loan Term: {confirmation.FinanceDetails.LoanTerm} months");

                                        payment.Item()
                                            .Text($"Estimated Monthly Payment: {confirmation.FinanceDetails.EstimatedPayment:N2}")
                                            .Bold();
                                    }
                                    else
                                    {
                                        payment.Item()
                                            .Text("Payment Type: Full Cash Payment")
                                            .Bold();
                                    }
                                });

                            // Thank You Message
                            column.Item()
                                .PaddingTop(20)
                                .AlignCenter()
                                .Text("Thank you for choosing Car's Wroom!")
                                .FontSize(15)
                                .Bold()
                                .FontColor(Colors.Blue.Darken2);
                        });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text("Car's Wroom | Purchase Confirmation")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);
                });
            });

            return pdf.GeneratePdf();
        }
    }
}