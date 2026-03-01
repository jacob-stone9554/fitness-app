import { ButtonLink, Container } from "../ui/ui.tsx";
import icon from "../assets/icon.svg";


function Navbar() {
    return (
        <header className="sticky top-0 z-50 border-b border-white/10 bg-neutral-950/70 backdrop-blur">
            <Container className="flex items-center justify-between py-3">
                <div className="flex items-center gap-2">
                    <img src={icon} alt="Fitness App Logo" className="h-8 w-8" />
                    <span className="text-sm font-semibold tracking-wide">Fitness App</span>
                </div>

                <nav className="hidden items-center gap-6 md:flex">
                    <a className="text-sm text-white/70 hover:text-white" href="#features">Features</a>
                    <a className="text-sm text-white/70 hover:text-white" href="#how">How it works</a>
                    <a className="text-sm text-white/70 hover:text-white" href="#pricing">Pricing</a>
                </nav>

                <div className="flex items-center gap-3">
                    <ButtonLink to="/login" variant="ghost" size="sm">Log in</ButtonLink>
                    <ButtonLink to="/register" variant="primary" size="sm">Get started</ButtonLink>
                </div>
            </Container>
        </header>
    );
}

export default Navbar;