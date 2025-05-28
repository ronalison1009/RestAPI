# 💶 ATM Denomination Calculator

A console-based C# application that simulates how an ATM dispenses requested amounts using available denominations: **10 EUR**, **50 EUR**, and **100 EUR**.

---

## 🧰 Features

- Accepts user input for one or more payout amounts (space-separated)
- Supports only **multiples of 10 EUR**
- Displays **all valid combinations** of 10, 50, and 100 EUR bills for each amount
- Suggests the **nearest valid amount** if the input is not a multiple of 10
- Allows the user to accept or reject the suggested amount
- Gracefully handles invalid or empty input

---

## 📦 Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) (.NET 6.0 or later)

---

## 🚀 How to Run

1. **Clone or Download the Repository**

   ```bash
   git clone https://github.com/ronalison1009/RestAPI.git
   cd atm-denomination-calculator
