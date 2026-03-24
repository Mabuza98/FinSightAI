import Sidebar from "./Sidebar";
import { Outlet } from "react-router-dom";

function Layout() {
    return (
        <div className="flex">
            <Sidebar />

            <div className="flex-1 bg-gray-800 min-h-screen p-6 text-white">
                <Outlet />
            </div>
        </div>
    );
}

export default Layout;