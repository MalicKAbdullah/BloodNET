var connection = new signalR.HubConnectionBuilder().withUrl("/bloodRequestsHub").build();

// Handle the ReceiveBloodRequest event
connection.on("ReceiveBloodRequest", function (bloodgroup, dtime, rname, raddress) {
    Toastify({
        text: `
            <div style="color: black; font-family: 'BloodNET-Regular', Arial, sans-serif; padding: 15px; border-radius: 8px; border: 1px solid #ddd; background-color: #f9f9f9;">
                <span style="font-size: 16px; color: #555;"><strong>🩸 Blood Request:</strong> ${bloodgroup}</span><br>
                <span style="font-size: 16px; color: #555;"><strong>📍 Location:</strong> ${raddress}</span><br>
                <span style="font-size: 16px; color: #555;"><strong>🕒 Time:</strong> ${new Date(dtime).toLocaleString()}</span><br>
                <span style="font-size: 16px; color: #555;"><strong>👤 Name:</strong> ${rname}</span>
            </div>
        `,
        duration: 8000,
        gravity: "bottom",
        position: "right",
        backgroundColor: "transparent", // Ensure background is transparent
        stopOnFocus: true,
        escapeMarkup: false, // Allow HTML formatting
        className: "custom-toast" // Use a custom class for additional styling
    }).showToast();
});

connection.start().catch(function (err) {
    console.error(err.toString());
});
