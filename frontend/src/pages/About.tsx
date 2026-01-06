import React from 'react';
import { t } from '../i18n/translations';
import './Settings.css'; // Reuse settings styles for consistency
import appLogo from '../assets/app_logo.png';
import { useUI } from '../context/UIContext';

const About: React.FC = () => {
    const { toggleKofi } = useUI();

    return (
        <div className="settings">
            <div className="settings-section">
                <div style={{ textAlign: 'center', marginBottom: '1.25rem' }}>
                    <div onClick={toggleKofi} style={{ cursor: 'pointer', display: 'inline-block' }}>
                        <img src={appLogo} alt="Playerr" style={{ width: '100px', height: 'auto', marginBottom: '0.75rem' }} />
                    </div>
                    <h3>Playerr v0.2.2 (Beta)</h3>
                </div>

                <div className="settings-section" style={{ border: 'none', padding: 0, backgroundColor: 'transparent' }}>

                    <p className="settings-description" style={{ fontSize: '0.95rem', lineHeight: '1.5', marginBottom: '1.25rem' }}>
                        {t('aboutMainDesc')}
                    </p>

                    <div style={{ marginBottom: '1.25rem' }}>
                        <h4 style={{ color: '#cdd6f4', fontSize: '1rem', marginBottom: '0.75rem', fontWeight: 600 }}>{t('featuresTitle')}</h4>
                        <ul style={{ listStyleType: 'none', padding: 0, color: '#a6adc8', lineHeight: '1.6', fontSize: '0.9rem' }}>
                            <li>• {t('featureScanning')}</li>
                            <li>• {t('featureIntegration')}</li>
                            <li>• {t('featureCrossPlatform')}</li>
                        </ul>
                    </div>

                    <div style={{ marginBottom: '1.25rem' }}>
                        <h4 style={{ color: '#cdd6f4', fontSize: '1rem', marginBottom: '0.75rem', fontWeight: 600 }}>{t('roadmapTitle')}</h4>
                        <ul style={{ listStyleType: 'none', padding: 0, color: '#a6adc8', lineHeight: '1.6', fontSize: '0.9rem' }}>
                            <li>• {t('roadmapBazzite')}</li>
                            <li>• {t('roadmapUsb')}</li>
                        </ul>
                    </div>

                    <div style={{ borderTop: '1px solid #313244', paddingTop: '1.25rem', marginTop: '1.5rem' }}>
                        <p className="settings-description" style={{ marginBottom: '0.5rem' }}>
                            {t('developedBy')} <strong style={{ color: '#cdd6f4' }}>{t('developedByStrong')}</strong>.
                        </p>
                        <p className="settings-description" style={{ fontStyle: 'italic', opacity: 0.8, marginBottom: '1rem' }}>
                            {t('supportText')}
                        </p>

                        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem', marginBottom: '0.75rem' }}>
                            <a href="https://github.com/Maikboarder" target="_blank" rel="noopener noreferrer" style={{ opacity: 0.8, transition: 'opacity 0.2s' }}>
                                <img src="https://img.shields.io/github/followers/Maikboarder?label=Follow&style=social" alt="GitHub Follow" />
                            </a>
                        </div>
                        <p className="settings-description" style={{ fontSize: '0.8rem', opacity: 0.5, marginBottom: 0 }}>
                            {t('license')}
                        </p>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default About;
