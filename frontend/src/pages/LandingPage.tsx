import Navbar from "../components/Navbar.tsx";
import Hero from "../components/Hero.tsx";
import Stats from "../components/Stats.tsx";
import Features from "../components/Features.tsx";
import HowItWorks from "../components/HowItWorks.tsx";
import CTA from "../components/CTA.tsx";
import Footer from "../components/Footer.tsx";

function LandingPage() {
    return(
        <div className="min-h-screen bg-neutral-950 text-neutral-50">
            <Navbar />
            <main>
                <Hero />
                <Stats />
                <Features />
                <HowItWorks />
                <CTA />
                <Footer />
            </main>
        </div>
    );
}

export default LandingPage;