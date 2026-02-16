function Footer() {
    return (
        <footer className="border-t border-white/10">
            <div className="mx-auto flex max-w-6xl flex-col gap-4 px-4 py-10 md:flex-row md:items-center md:justify-between">
                <div className="text-sm text-white/60">© {new Date().getFullYear()} Fitness App</div>
                <div className="flex gap-5 text-sm text-white/60">
                    <a className="hover:text-white" href="#">
                        Privacy
                    </a>
                    <a className="hover:text-white" href="#">
                        Terms
                    </a>
                    <a className="hover:text-white" href="#">
                        Contact
                    </a>
                </div>
            </div>
        </footer>
    );
}

export default Footer;