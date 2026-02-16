import {Card, Container} from "../ui/ui.tsx";

function Stats() {
    const stats = [
        { label: "Workout logging", value: "Fast" },
        { label: "Progress tracking", value: "Clean" },
        { label: "Noise & bloat", value: "None" },
    ];

    return (
        <section className="border-y border-white/10 bg-white/[0.02]">
            <Container className="grid grid-cols-1 gap-6 py-10 sm:grid-cols-3">
                {stats.map((s) => (
                    <Card key={s.label} className="p-5">
                        <div className="text-2xl font-semibold">{s.value}</div>
                        <div className="mt-1 text-sm text-white/60">{s.label}</div>
                    </Card>
                ))}
            </Container>
        </section>
    );
}

export default Stats;