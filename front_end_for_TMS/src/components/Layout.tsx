import { Outlet, NavLink } from "react-router-dom";

const Layout = () => {
  const linkStyle = ({ isActive }: { isActive: boolean }) => ({
    textDecoration: "none",
    color: isActive ? "#2563eb" : "#4b5563",
    fontWeight: isActive ? "600" : "400",
    fontSize: "0.9rem",
  });

  return (
    <div
      style={{ minHeight: "100vh", display: "flex", flexDirection: "column" }}
    >
      <nav
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          padding: "1rem 2rem",
          background: "white",
          borderBottom: "1px solid #e5e7eb",
        }}
      >
        <div
          style={{ fontWeight: "bold", fontSize: "1.2rem", color: "#2563eb" }}
        >
          Brand
        </div>
        <div style={{ display: "flex", gap: "1.5rem" }}>
          <NavLink title="Home" to="/" style={linkStyle}>
            Home
          </NavLink>
          <NavLink title="Login" to="/login" style={linkStyle}>
            Login
          </NavLink>
          <NavLink title="Register" to="/register" style={linkStyle}>
            Register
          </NavLink>
        </div>
      </nav>
      <main
        style={{
          flex: 1,
          display: "flex",
          justifyContent: "center",
          alignItems: "flex-start",
          paddingTop: "4rem",
        }}
      >
        <Outlet />
      </main>
    </div>
  );
};

export default Layout;
