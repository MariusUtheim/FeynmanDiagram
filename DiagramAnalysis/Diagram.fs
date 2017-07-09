module Diagram

type Particle = Electron | Muon | Tauon | Photon

type Vertex = { Type: VertexType; Particle: Particle }
