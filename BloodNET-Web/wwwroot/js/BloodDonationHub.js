var connection = new signalR.HubConnectionBuilder().withUrl("/bloodDonationHub").build();

// Handle the ReceiveDonation event
connection.on("ReceiveDonation", function (donorId) {
    Toastify({
        text: `
            <div style="color: black; font-family: 'BloodNET-Regular', Arial, sans-serif; padding: 15px; border-radius: 8px; border: 1px solid #ddd; background-color: #f9f9f9;">
                <span style="font-size: 16px; color: #555;"><strong>💉 ${donorId}</strong> made a Donation</span><br><br>
                <span style="font-size: 14px; color: #333;">"Your generosity saves lives. Thank you for making a difference!"</span>
            </div>
        `,
        duration: 8000,
        gravity: "bottom",
        position: "right",
        backgroundColor: "transparent", // Ensure the Toastify background is transparent
        stopOnFocus: true,
        escapeMarkup: false, // Allow HTML formatting
        className: "custom-toast" // Add a custom class for additional styling
    }).showToast();
});

connection.start().catch(function (err) {
    console.error(err.toString());
});
