I’ve got it all noted down. Here's the detailed list of issues:

1. In the Orders tab, online orders automatically enter the "Pending" section by default. There is currently no constraint limiting how long an order can stay in "Pending." A 24-hour constraint is needed—if the order remains in "Pending" beyond 24 hours, it should automatically cancel. Before that, the system should email a reminder to the user as a background function. Once the time limit expires, it should move to "Canceled."

2. In the Payment Verification section, when a user uploads payment proof (image or file under 10 MB), the system should automatically move that order from "Pending" to "Payment Verification." No manual intervention is required. The admin will then verify the payment details (amount, account number, etc.). If verified, the admin will set the order to "Processing." If there is a discrepancy, the admin will mark it as "On Hold." All validations and status changes must be automatic once initiated by the user’s proof submission.

These two are the main issues detected. Let me know if there are more.
