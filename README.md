# Primes With Circles

**Visualizing Prime Numbers through Kinetic Motion and Resonance.**

<img width="1225" height="859" alt="image" src="https://github.com/user-attachments/assets/13f3dd7c-6453-4510-bcaf-09c46d7ffad3" />

---

This project is a C# / WPF application that explores the emergence of prime numbers using rotating circles. Instead of a static sieve, it treats primality as a phenomenon of phase alignment and periodic motion.

---

License: GPL-3.0-or-later

### 🚀 Features
* **Kinetic Visualization:** Watch prime numbers "emerge" as rotating phases.
* **Pure C# / WPF:** The animation is driven by CompositionTarget.Rendering.
* **No External Dependencies:** Zero third-party libraries; just clean, optimized code.

### 🧠 How it works: Cyclic Prime Emergence Algorithm

The core logic follows a geometric progression where primes are identified through synchronized rotations:

1. **Initialization:** Start with two concentric circles of radius $r$ and $2r$.
2. **Motion:** Rotate them with constant linear speed from the same starting point.
3. **Iteration:** Each time the first circle completes a full rotation, increment a counter $n$.
4. **Detection:** If no other circle finishes its rotation at the exact same time as the first, then the current counter value $n$ is **prime**.
5. **Growth:** If $n$ is found to be prime, add a new concentric circle with a radius of $n \times r$.

This creates an open-ended, geometric simulation where primality emerges from phase alignment. Unlike the traditional Sieve of Eratosthenes, this approach requires no divisions and no predefined upper limit; it is a streaming visualization of number theory in motion.

### 💡 Key Differences from the Sieve of Eratosthenes

While inspired by classical number theory, this implementation introduces two fundamental shifts:
* **Zero Divisions:** Primality is determined through geometric resonance and phase alignment rather than arithmetic division ($n \pmod d$).
* **Infinite Streaming:** Unlike the static "batch" nature of Eratosthenes' sieve, this is an incremental process. It requires no predefined upper limit, allowing primes to emerge continuously as the simulation evolves.

### 🛠️ Technical Details
* **Language:** C#
* **Framework:** .NET / WPF
* **Rendering:** High-performance animation driven by CompositionTarget.Rendering, ensuring frame-perfect synchronization with the monitor's refresh rate.

---

You can view a presentation at https://youtu.be/yKuR0GW0x7k

or find more details at https://nkode.gr/EN/articles/278/when-prime-numbers-emerge-from-motion

<img width="1227" height="856" alt="image" src="https://github.com/user-attachments/assets/d261e616-2cef-43f3-b7ff-86c74cbf5428" />

<img width="1227" height="820" alt="image" src="https://github.com/user-attachments/assets/d1b45f70-b9ec-4d27-a86e-fa269f418724" />




