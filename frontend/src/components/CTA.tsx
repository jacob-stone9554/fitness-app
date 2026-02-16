function CTA() {
    return (
        <section id="pricing" className="border-t border-white/10">
            <div className="mx-auto max-w-6xl px-4 py-16 md:py-20">
                <div className="rounded-3xl border border-white/10 bg-white/5 p-8 md:p-12">
                    <h3 className="text-2xl font-semibold md:text-3xl">Ready to start tracking?</h3>
                    <p className="mt-3 max-w-2xl text-white/70">
                        Get a clean dashboard, fast workout logging, and progress you can actually
                        understand.
                    </p>
                    <div className="mt-7 flex flex-col gap-3 sm:flex-row">
                        <a
                            href="/#/register"
                            className="inline-flex items-center justify-center rounded-xl bg-white px-5 py-3 text-sm font-semibold text-neutral-950 hover:bg-white/90"
                        >
                            Get started
                        </a>
                        <a
                            href="/#/login"
                            className="inline-flex items-center justify-center rounded-xl border border-white/15 bg-white/5 px-5 py-3 text-sm font-semibold text-white hover:bg-white/10"
                        >
                            Log in
                        </a>
                    </div>
                    <p className="mt-4 text-xs text-white/50">Sign up with a username and password</p>
                </div>
            </div>
        </section>
    );
}

export default CTA;