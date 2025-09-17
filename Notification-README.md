# 📲 Flutter Notifications with SignalR and JWT

This document explains step by step how the Flutter team can implement **real-time notifications** using **SignalR Hub** with **JWT authentication**.

---

## 🔹 1. Overview
- The backend provides a **SignalR Hub** endpoint.
- Every user is assigned to a **role-based group** (e.g., `Admin`, `Client`, `Manager`) using the **JWT token**.
- The Flutter client:
  1. Passes the **JWT token** when connecting.
  2. Establishes a **SignalR connection**.
  3. Subscribes to the **ReceiveMessage** event.
  4. Displays notifications in the UI.

⚠️ **Note:** Roles are case-sensitive.  
Example: `"Client"` ≠ `"client"`. Always send the correct case.

---

## 🔹 2. Requirements
- [Flutter SDK](https://docs.flutter.dev/get-started/install)
- [signalr_netcore](https://pub.dev/packages/signalr_netcore) package
- Valid **JWT token** (retrieved after login from the backend)

---

## 🔹 3. Install Dependencies
In `pubspec.yaml` add:

```yaml
dependencies:
  flutter:
    sdk: flutter
  signalr_netcore: ^1.3.7
